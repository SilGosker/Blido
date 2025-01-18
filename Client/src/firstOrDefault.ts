import {DotNetObjectReference} from "./DotNetObjectReference";
import {connectToDatabase} from "./connectToDatabase";

export interface firstOrDefaultInterface {
    (database: string, objectStore: string, currentVersion: number, transactionConditions: DotNetObjectReference) : Promise<unknown>
};

export function firstOrDefault(database: string, objectStore: string, currentVersion: number, transactionConditions: DotNetObjectReference): Promise<unknown> {
    return new Promise(async (resolve, reject) => {
        const connection = await connectToDatabase(database, currentVersion);
        const db: IDBDatabase = (connection.target as any).result;
        const transaction = db.transaction(objectStore, 'readwrite').objectStore(objectStore).openCursor();

        transaction.addEventListener('success', async (event) => {
            const cursor = (event.target as any).result as IDBCursorWithValue;
            const object = cursor.value;

            if (await transactionConditions.invokeMethodAsync('FullFillsConditions', object)) {
                resolve(object);
                return;
            }

            cursor.continue();
        });

        transaction.addEventListener('error', reject);
    });

}