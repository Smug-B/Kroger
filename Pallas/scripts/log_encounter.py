import argparse
import datetime
import sqlite3

from utils import ensure_working_directory, get_parent_id, get_database


def create_encounter_database(connection: sqlite3.Connection, cursor: sqlite3.Cursor) -> None:
    cursor.execute("""
                   CREATE TABLE IF NOT EXISTS encounters_map
                   (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       uri TEXT UNIQUE,
                       create_time DATETIME,
                       encounter_time DATETIME,
                       visibility INTEGER,
                       source TEXT,
                       source_confidence INTEGER,
                       content TEXT
                   )
                   """)
    connection.commit()

def create_comments_database(connection: sqlite3.Connection, cursor: sqlite3.Cursor) -> None:
    create_encounter_database(connection, cursor)
    cursor.execute("""
                   CREATE TABLE IF NOT EXISTS comments_map
                   (
                       comment_id INTEGER PRIMARY KEY AUTOINCREMENT,
                       parent_id INTEGER,
                       uri TEXT UNIQUE,
                       FOREIGN KEY (parent_id) REFERENCES encounters_map(id) ON DELETE CASCADE
                   )
                   """)
    connection.commit()

def create_encounter_entry(cursor: sqlite3.Cursor, parameters) -> None:
    cursor.execute(
        """
        INSERT INTO encounters_map
        (uri, create_time, encounter_time, visibility, source, source_confidence, content) 
        VALUES (?, ?, ?, ?, ?, ?, ?)
        ON CONFLICT(uri) DO UPDATE SET
            create_time = excluded.create_time,
            encounter_time = excluded.encounter_time,
            visibility = excluded.visibility,
            source = excluded.source,
            source_confidence = excluded.source_confidence,
            content = excluded.content
        """,
        parameters
    )

def create_comments_entry(cursor: sqlite3.Cursor, parameters) -> None:
    cursor.execute(
        """
        INSERT INTO comments_map
        (parent_id, uri) 
        VALUES (?, ?)
        ON CONFLICT(uri) DO UPDATE SET
            parent_id = excluded.parent_id
        """,
        parameters
    )

def main():
    ensure_working_directory("Kroger")

    parser: argparse.ArgumentParser = argparse.ArgumentParser()
    parser.add_argument("uri",
                        type=str,
                        help="URI leading to the post.")
    parser.add_argument("parent_uri",
                        type=str,
                        help="If the post is a comment, uri leading to the parent post."
                             "If the post is a top-level post, uri leading to itself.")
    parser.add_argument("create_time",
                        type=str,
                        help="Post's creation time in ISO-8601 format.")
    parser.add_argument("visibility",
                        type=int,
                        help="Calculated post visibility.")
    parser.add_argument("source",
                        type=str,
                        help="inferred post source.")
    parser.add_argument("source_confidence",
                        type=int,
                        help="Confidence of inferred post source.")
    parser.add_argument("content",
                        type=str,
                        help="Pithy summary of post's content.")
    parser.add_argument("--run_name",
                        type=str,
                        required=False,
                        default="",
                        help="Name of folder where the encounters database will be stored.")
    args = parser.parse_args()

    now = datetime.datetime.now()
    date = now.strftime('%Y-%m-%d')

    uri: str = args.uri
    parent_uri: str = args.parent_uri
    is_comment: bool = uri != parent_uri
    create_time: str = args.create_time
    encounter_time: str = now.strftime('%Y-%m-%d %H:%M:%S')
    visibility: int = args.visibility
    source: str = args.source
    source_confidence: int = args.source_confidence
    content: str = args.content
    run_name: str = args.run_name

    connection: sqlite3.Connection = get_database(run_name)
    cursor: sqlite3.Cursor = connection.cursor()

    create_encounter_database(connection, cursor)
    create_encounter_entry(cursor, parameters=(uri, create_time, encounter_time, visibility, source, source_confidence, content))

    if is_comment:
        cursor.execute("PRAGMA foreign_keys = ON;")
        create_comments_database(connection, cursor)
        parent_id: int = get_parent_id(cursor, parent_uri)
        create_comments_entry(cursor, parameters=(parent_id, uri))
    connection.commit()
    connection.close()


if __name__ == "__main__":
    main()
