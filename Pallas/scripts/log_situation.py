import argparse
import sqlite3


from utils import ensure_working_directory, get_database


def create_situation_table(connection: sqlite3.Connection, cursor: sqlite3.Cursor) -> None:
    cursor.execute("""
                   CREATE TABLE IF NOT EXISTS situation_map
                   (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       situation_title TEXT UNIQUE,
                       situation_description TEXT,
                       situation_begin_date DATETIME,
                       situation_reason TEXT,
                       situation_reason_EQ TEXT,
                       situation_visibility INTEGER,
                       situation_visibility_EQ TEXT,
                       situation_visibility_reason TEXT
                   )
                   """)
    connection.commit()

def create_situation_entry(cursor: sqlite3.Cursor, parameters) -> None:
    cursor.execute(
        """
        INSERT INTO situation_map
        (situation_title, situation_description, situation_begin_date, situation_reason, situation_reason_EQ, 
         situation_visibility, situation_visibility_EQ, situation_visibility_reason) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?)
        ON CONFLICT(situation_title) DO UPDATE SET
            situation_description = CASE 
                WHEN excluded.situation_description != '' AND excluded.situation_description IS NOT NULL 
                THEN excluded.situation_description 
                ELSE situation_description 
            END,
            situation_begin_date = CASE 
                WHEN excluded.situation_begin_date != '' AND excluded.situation_begin_date IS NOT NULL 
                THEN excluded.situation_begin_date 
                ELSE situation_begin_date 
            END,
            situation_reason = CASE 
                WHEN excluded.situation_reason != '' AND excluded.situation_reason IS NOT NULL 
                THEN excluded.situation_reason 
                ELSE situation_reason 
            END,
            situation_reason_EQ = CASE 
                WHEN excluded.situation_reason_EQ != '' AND excluded.situation_reason_EQ IS NOT NULL 
                THEN excluded.situation_reason_EQ 
                ELSE situation_reason_EQ 
            END,
            situation_visibility = CASE 
                WHEN excluded.situation_visibility != -1 AND excluded.situation_visibility IS NOT NULL 
                THEN excluded.situation_visibility 
                ELSE situation_visibility 
            END,
            situation_visibility_EQ = CASE 
                WHEN excluded.situation_visibility_EQ != '' AND excluded.situation_visibility_EQ IS NOT NULL 
                THEN excluded.situation_visibility_EQ 
                ELSE situation_visibility_EQ 
            END,
            situation_visibility_reason = CASE 
                WHEN excluded.situation_visibility_reason != '' AND excluded.situation_visibility_reason IS NOT NULL 
                THEN excluded.situation_visibility_reason 
                ELSE situation_visibility_reason 
            END
        """,
        parameters
    )

def main():
    ensure_working_directory("Kroger")

    parser: argparse.ArgumentParser = argparse.ArgumentParser()
    parser.add_argument("situation_title",
                        type=str,
                        help="Name of the situation. No spaces. This should be able to be used as a file name.")
    parser.add_argument("--situation_description",
                        type=str,
                        required=False,
                        default="",
                        help="A brief summary of the situation.")
    parser.add_argument("--situation_begin_date",
                        type=str,
                        required=False,
                        default="",
                        help="An estimated date, in ISO-8601 format, at which the situation began.")
    parser.add_argument("--situation_reason",
                        type=str,
                        required=False,
                        default="",
                        help="The reason behind the situation.")
    parser.add_argument("--situation_reason_EQ",
                        type=str,
                        required=False,
                        default="",
                        help="The evidence quality associated with the given situation_reason.")
    parser.add_argument("--situation_visibility",
                        type=int,
                        required=False,
                        default=-1,
                        help="A numerical score approximating how visible the situation is to the public as a whole.")
    parser.add_argument("--situation_visibility_EQ",
                        type=str,
                        required=False,
                        default="",
                        help="The evidence quality associated with the given situation_visibility.")
    parser.add_argument("--situation_visibility_reason",
                        type=str,
                        required=False,
                        default="",
                        help="Reasoning behind the given situation_visibility.")
    parser.add_argument("--run_name",
                        type=str,
                        required=False,
                        default="",
                        help="Name of folder where the database will be stored.")
    args = parser.parse_args()

    situation_title: str = args.situation_title
    situation_description: str = args.situation_description
    situation_begin_date: str = args.situation_begin_date
    situation_reason: str = args.situation_reason
    situation_reason_EQ: str = args.situation_reason_EQ
    situation_visibility: int = args.situation_visibility
    situation_visibility_EQ: str = args.situation_visibility_EQ
    situation_visibility_reason: str = args.situation_visibility_reason
    run_name: str = args.run_name

    connection: sqlite3.Connection = get_database(run_name)
    cursor: sqlite3.Cursor = connection.cursor()

    create_situation_table(connection, cursor)
    create_situation_entry(cursor, parameters=(situation_title,
                                               situation_description,
                                               situation_begin_date,
                                               situation_reason,
                                               situation_reason_EQ,
                                               situation_visibility,
                                               situation_visibility_EQ,
                                               situation_visibility_reason))
    connection.commit()
    connection.close()


if __name__ == "__main__":
    main()
