import argparse
import sqlite3


from utils import ensure_working_directory, get_database


def create_es_table(connection: sqlite3.Connection, cursor: sqlite3.Cursor) -> None:
    cursor.execute("""
                   CREATE TABLE IF NOT EXISTS encounter_situation_map
                   (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       encounter_id INTEGER NOT NULL,
                       situation_id INTEGER NOT NULL,
                       FOREIGN KEY (encounter_id) REFERENCES encounters_map(id) ON DELETE CASCADE
                       FOREIGN KEY (situation_id) REFERENCES situation_map(id) ON DELETE CASCADE
                       UNIQUE (encounter_id, situation_id)
                   )
                   """)
    connection.commit()

def create_es_entry(cursor: sqlite3.Cursor, parameters) -> None:
    cursor.execute(
        """
        INSERT INTO encounter_situation_map
        (encounter_id, situation_id) 
        VALUES (?, ?)
        """,
        parameters
    )

def main():
    ensure_working_directory("Kroger")

    parser: argparse.ArgumentParser = argparse.ArgumentParser()
    parser.add_argument("uri",
                        type=str,
                        help="URI leading to the post.")
    parser.add_argument("situation_title",
                        type=str,
                        help="URI leading to the post.")
    parser.add_argument("--run_name",
                        type=str,
                        required=False,
                        default="",
                        help="Name of folder where the database will be stored.")
    args = parser.parse_args()

    uri: str = args.uri
    situation_title: str = args.situation_title
    run_name: str = args.run_name

    connection: sqlite3.Connection = get_database(run_name)
    cursor: sqlite3.Cursor = connection.cursor()

    create_es_table(connection, cursor)
    create_es_entry(cursor, parameters=(uri, situation_title))
    connection.commit()
    connection.close()


if __name__ == "__main__":
    main()
