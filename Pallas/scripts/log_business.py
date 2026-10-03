import argparse
import sqlite3


from utils import ensure_working_directory, get_database, get_parent_id


def create_business_database(connection: sqlite3.Connection, cursor: sqlite3.Cursor) -> None:
    cursor.execute("""
                   CREATE TABLE IF NOT EXISTS business_map
                   (
                       id INTEGER PRIMARY KEY AUTOINCREMENT,
                       encounter_id INTEGER UNIQUE,
                       uri TEXT UNIQUE,
                       information_value INTEGER,
                       information_value_EQ TEXT,
                       information_value_reason TEXT,
                       business_relevance INTEGER,
                       business_relevance_EQ TEXT,
                       business_relevance_reason TEXT,
                       business_implications INTEGER,
                       business_implications_EQ TEXT,
                       business_implications_reason TEXT,
                       FOREIGN KEY (encounter_id) REFERENCES encounters_map(id) ON DELETE CASCADE
                   )
                   """)
    connection.commit()

def create_business_entry(cursor: sqlite3.Cursor, parameters) -> None:
    cursor.execute(
        """
        INSERT INTO business_map
        (encounter_id, uri, 
        information_value, information_value_EQ, information_value_reason, 
        business_relevance, business_relevance_EQ, business_relevance_reason, 
        business_implications, business_implications_EQ, business_implications_reason) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        ON CONFLICT(uri) DO UPDATE SET
            encounter_id = excluded.encounter_id,
            information_value = excluded.information_value,
            information_value_EQ = excluded.information_value_EQ,
            information_value_reason = excluded.information_value_reason,
            business_relevance = excluded.business_relevance,
            business_relevance_EQ = excluded.business_relevance_EQ,
            business_relevance_reason = excluded.business_relevance_reason,
            business_implications = excluded.business_implications,
            business_implications_EQ = excluded.business_implications_EQ,
            business_implications_reason = excluded.business_implications_reason
        """,
        parameters
    )


def main():
    ensure_working_directory("Kroger")

    parser: argparse.ArgumentParser = argparse.ArgumentParser()
    parser.add_argument("uri",
                        type=str,
                        help="URI leading to the post.")
    parser.add_argument("information_value",
                        type=int,
                        help="A numerical score of the information value contained in the given URI.")
    parser.add_argument("information_value_EQ",
                        type=str,
                        help="The evidence quality associated with the information_value.")
    parser.add_argument("information_value_reason",
                        type=str,
                        help="Reasoning behind the given information_value.")
    parser.add_argument("business_relevance",
                        type=int,
                        help="A numerical score of the business relevance of the given URI.")
    parser.add_argument("business_relevance_EQ",
                        type=str,
                        help="The evidence quality associated with the business_relevance.")
    parser.add_argument("business_relevance_reason",
                        type=str,
                        help="Reasoning behind the given business_relevance.")
    parser.add_argument("business_implications",
                        type=int,
                        help="A numerical score of the information value contained in the given URI.")
    parser.add_argument("business_implications_EQ",
                        type=str,
                        help="The evidence quality associated with the business_implications.")
    parser.add_argument("business_implications_reason",
                        type=str,
                        help="Reasoning behind the given business_implications.")
    parser.add_argument("--run_name",
                        type=str,
                        required=False,
                        default="",
                        help="Name of folder where the database will be stored.")
    args = parser.parse_args()

    uri: str = args.uri
    information_value: int = args.information_value
    information_value_eq: str = args.information_value_EQ
    information_value_reason: str = args.information_value_reason
    business_relevance: int = args.business_relevance
    business_relevance_eq: str = args.business_relevance_EQ
    business_relevance_reason: str = args.business_relevance_reason
    business_implications: int = args.business_implications
    business_implications_eq: str = args.business_implications_EQ
    business_implications_reason: str = args.business_implications_reason

    run_name: str = args.run_name

    connection: sqlite3.Connection = get_database(run_name)
    cursor: sqlite3.Cursor = connection.cursor()

    create_business_database(connection, cursor)
    parent_id: int = get_parent_id(cursor, uri)
    create_business_entry(cursor, parameters=(parent_id,
                                              uri,
                                              information_value,
                                              information_value_eq,
                                              information_value_reason,
                                              business_relevance,
                                              business_relevance_eq,
                                              business_relevance_reason,
                                              business_implications,
                                              business_implications_eq,
                                              business_implications_reason))
    connection.commit()
    connection.close()


if __name__ == "__main__":
    main()
