import datetime
import os
import sqlite3
from pathlib import Path


def ensure_working_directory(target_folder_name: str) -> Path:
    current = Path.cwd()
    for parent in [current] + list(current.parents):
        if parent.name == target_folder_name:
            os.chdir(parent)
            return parent
    raise FileNotFoundError(f"Could not find parent directory '{target_folder_name}' in the path hierarchy.")

def get_run_path(run_name_candidate: str) -> Path:
    now = datetime.datetime.now()
    date = now.strftime('%Y-%m-%d')
    run_name: str = date if run_name_candidate == "" else run_name_candidate
    run_path: Path = Path("Pallas", "run", run_name)
    run_path.mkdir(parents=True, exist_ok=True)
    return run_path

def get_database(run_path: Path | str) -> sqlite3.Connection:
    if isinstance(run_path, str):
        run_path = get_run_path(run_path)
    return sqlite3.connect(run_path.joinpath("encounters.db"))

def create_plain_encounter_entry(cursor: sqlite3.Cursor, uri: str) -> None:
    cursor.execute(
        """
        INSERT INTO encounters_map
        (uri) 
        VALUES (?)
        """,
        (uri,)
    )

def get_parent_id(cursor: sqlite3.Cursor, parent_uri: str) -> int:
    cursor.execute("SELECT id FROM encounters_map WHERE uri = ?", (parent_uri,))
    parent_row = cursor.fetchone()
    if parent_row is None:
        create_plain_encounter_entry(cursor, parent_uri)
        cursor.execute("SELECT id FROM encounters_map WHERE uri = ?", (parent_uri,))
        parent_row = cursor.fetchone()
    return parent_row[0]

if __name__ == "__main__":
    pass