import argparse
import sqlite3


from utils import ensure_working_directory, get_database, get_parent_id


def create_source_database(connection: sqlite3.Connection, cursor: sqlite3.Cursor) -> None:
    cursor.execute("""
                   CREATE TABLE IF NOT EXISTS source_map
                   (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       encounter_id INTEGER UNIQUE,
                       uri TEXT UNIQUE,
                       source_inference TEXT,
                       source_EQ TEXT,
                       inference_reasoning TEXT,
                       manager_EQ TEXT,
                       employee_EQ TEXT,
                       customer_EQ TEXT,
                       media_EQ TEXT,
                       other_EQ TEXT,
                       FOREIGN KEY (encounter_id) REFERENCES encounters_map(id) ON DELETE CASCADE
                   )
                   """)
    connection.commit()

def create_source_entry(cursor: sqlite3.Cursor, parameters) -> None:
    cursor.execute(
        """
        INSERT INTO source_map
        (encounter_id, uri, source_inference, source_EQ, inference_reasoning, manager_EQ, employee_EQ, customer_EQ, media_EQ, other_EQ) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        ON CONFLICT(uri) DO UPDATE SET
            encounter_id = excluded.encounter_id,
            source_inference = excluded.source_inference,
            source_EQ = excluded.source_EQ,
            inference_reasoning = excluded.inference_reasoning,
            manager_EQ = excluded.manager_EQ,
            employee_EQ = excluded.employee_EQ,
            customer_EQ = excluded.customer_EQ,
            media_EQ = excluded.media_EQ,
            other_EQ = excluded.other_EQ
        """,
        parameters
    )


def main():
    ensure_working_directory("Kroger")

    parser: argparse.ArgumentParser = argparse.ArgumentParser()
    parser.add_argument("uri",
                        type=str,
                        help="URI leading to the post.")
    parser.add_argument("source_inference",
                        type=str,
                        help="In inference as to who is the source behind the information contained within a post.")
    parser.add_argument("source_EQ",
                        type=str,
                        help="The evidence quality associated with the source_inference.")
    parser.add_argument("inference_reasoning",
                        type=str,
                        help="Reasoning behind the given source_inference.")
    parser.add_argument("manager_EQ",
                        type=str,
                        help="Evidence quality supporting a 'manager' inference.")
    parser.add_argument("employee_EQ",
                        type=str,
                        help="Evidence quality supporting an 'employee' inference.")
    parser.add_argument("customer_EQ",
                        type=str,
                        help="Evidence quality supporting a 'customer' inference.")
    parser.add_argument("media_EQ",
                        type=str,
                        help="Evidence quality supporting a 'media' inference.")
    parser.add_argument("other_EQ",
                        type=str,
                        help="Evidence quality supporting an 'other' inference.")
    parser.add_argument("--run_name",
                        type=str,
                        required=False,
                        default="",
                        help="Name of folder where the database will be stored.")
    args = parser.parse_args()

    uri: str = args.uri
    source_inference: str = args.source_inference
    source_eq: str = args.source_EQ
    inference_reasoning: str = args.inference_reasoning
    manager_eq: str = args.manager_EQ
    employee_eq: str = args.employee_EQ
    customer_eq: str = args.customer_EQ
    media_eq: str = args.media_EQ
    other_eq: str = args.other_EQ

    run_name: str = args.run_name

    connection: sqlite3.Connection = get_database(run_name)
    cursor: sqlite3.Cursor = connection.cursor()

    create_source_database(connection, cursor)
    parent_id: int = get_parent_id(cursor, uri)
    create_source_entry(cursor, parameters=(parent_id,
                                            uri,
                                            source_inference,
                                            source_eq,
                                            inference_reasoning,
                                            manager_eq,
                                            employee_eq,
                                            customer_eq,
                                            media_eq,
                                            other_eq))
    connection.commit()
    connection.close()


if __name__ == "__main__":
    main()
