import {DotNetObjectReference} from "../DotNetObjectReference";
import {connectToDatabase} from "../connectToDatabase";

export interface FirstOrDefaultInterface {
    (database: string, objectStore: string, currentVersion: number, transactionConditions: DotNetObjectReference): Promise<unknown>
}

export function firstOrDefault(database: string, objectStore: string, currentVersion: number, transactionConditions: DotNetObjectReference): Promise<unknown> {
    return new Promise(async (resolve, reject) => {
        const db = await connectToDatabase(database, currentVersion);
        const request = db.transaction(objectStore, 'readonly')
            .objectStore(objectStore).openCursor();

        request.onsuccess = () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                resolve(null);
                return;
            }

            const object = cursor.value;

            const fullFillsCondition = transactionConditions.invokeMethod('FullFillsConditions', object);

            if (fullFillsCondition) {
                resolve(object);
                return;
            }
            console.log(request);

            cursor.continue();
        };

        request.addEventListener('error', reject);
    });
}