import {startTransaction} from "./StartTransaction";

export async function startCursor(database: string, currentVersion: number, objectStore: string) {
    const db = await startTransaction(database, currentVersion, objectStore);
    return db.openCursor();
}