import {DotNetObjectReference} from "../DotNetObjectReference";
import {connectToDatabase} from "../connectToDatabase";

export interface FirstOrDefaultInterface {
    (database: string, objectStore: string, currentVersion: number, parsedExpression: string[]): Promise<unknown>
}

export function firstOrDefault(database: string, objectStore: string, currentVersion: number, parsedExpression: string[] | undefined) : Promise<unknown> {
    return new Promise(async (resolve, reject) => {
        const db = await connectToDatabase(database, currentVersion);
        const request = db.transaction(objectStore, 'readonly')
            .objectStore(objectStore).openCursor();

        let transactionConditions: ((entity: unknown) => boolean)[] = [_ => true];
        if (parsedExpression && parsedExpression.length) {
            transactionConditions = parsedExpression.map(x => eval(x) as (entity: unknown) => boolean);
        }

        request.onsuccess = () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                resolve(null);
                return;
            }

            const object = cursor.value;
            const fullFillsCondition = transactionConditions.every(x => x(object));
            if (fullFillsCondition) {
                resolve(object);
                return;
            }

            cursor.continue();
        };

        request.addEventListener('error', reject);
    });
}