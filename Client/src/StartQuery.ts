import {connectToDatabase} from "./ConnectToDatabase";

export async function startQuery(database: string, currentVersion: number, objectStore: string) {
    const db = await connectToDatabase(database, currentVersion);

    return db
        .transaction(objectStore, 'readonly')
        .objectStore(objectStore)
        .openCursor();
}