import {connectToDatabase} from "./ConnectToDatabase";

export async function startCursor(database: string, currentVersion: number, objectStore: string) {
    const db = await connectToDatabase(database, currentVersion);

    return db
        .transaction(objectStore, 'readonly')
        .objectStore(objectStore)
        .openCursor();
}

export async function startTransaction(database: string, currentVersion: number, objectStore: string, readonly = true) {
    const db = await connectToDatabase(database, currentVersion);

    return db
        .transaction(objectStore, readonly ? 'readonly' : "readwrite")
        .objectStore(objectStore);
}