import argparse
import sqlite3


from scripts.utils import ensure_working_directory, get_run_path, get_database, get_parent_id


def create_visibility_database(connection: sqlite3.Connection, cursor: sqlite3.Cursor) -> None:
    cursor.execute("""
                   CREATE TABLE IF NOT EXISTS visibility_map
                   (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       encounter_id INTEGER,
                       uri TEXT UNIQUE,
                       views INTEGER,
                       likes INTEGER,
                       reposts INTEGER,
                       replies INTEGER,
                       FOREIGN KEY (encounter_id) REFERENCES encounters_map(id) ON DELETE CASCADE
                   )
                   """)
    connection.commit()

def create_visibility_entry(cursor: sqlite3.Cursor, parameters) -> None:
    cursor.execute(
        """
        INSERT INTO visibility_map
        (encounter_id, uri, views, likes, reposts, replies) 
        VALUES (?, ?, ?, ?, ?)
        ON CONFLICT(uri) DO UPDATE SET
            encounter_id = excluded.encounter_id,
            views = excluded.views,
            likes = excluded.likes,
            reposts = excluded.reposts,
            replies = excluded.replies,
        """,
        parameters
    )


def main():
    ensure_working_directory("Kroger")

    parser: argparse.ArgumentParser = argparse.ArgumentParser()
    parser.add_argument("uri",
                        type=str,
                        help="URI leading to the post.")
    parser.add_argument("views",
                        type=int,
                        help="The number of views associated with the post."
                             "Negative numbers indicate unavailable metric.")
    parser.add_argument("likes",
                        type=int,
                        help="The number of likes associated with the post.")
    parser.add_argument("reposts",
                        type=str,
                        help="The number of reposts associated with the post.")
    parser.add_argument("replies",
                        type=int,
                        help="The number of replies associated with the post.")
    parser.add_argument("--run_name",
                        type=str,
                        required=False,
                        default="",
                        help="Name of folder where the database will be stored.")
    args = parser.parse_args()

    uri: str = args.uri
    views: int = args.views
    likes: str = args.likes
    reposts: int = args.reposts
    replies: str = args.replies
    run_name: str = args.run_name

    connection: sqlite3.Connection = get_database(run_name)
    cursor: sqlite3.Cursor = connection.cursor()

    create_visibility_database(connection, cursor)
    parent_id: int = get_parent_id(cursor, uri)
    create_visibility_entry(cursor, parameters=(parent_id, uri, views, likes, reposts, replies, run_name))
    connection.commit()
    connection.close()


if __name__ == "__main__":
    main()
