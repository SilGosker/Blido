import {processQueryArguments} from "../ProcessQueryArguments";
import {startTransaction} from "../StartTransaction";
import {startCursor} from "../StartCursor";

export function toArray(json: string) : Promise<unknown[]> {
    return new Promise(async (resolve, reject) => {
        const args = processQueryArguments(json);

        if (!args.hasFilters) {
            const transaction = await startTransaction(args.databaseName, args.currentVersion, args.objectStoreName);
            const request = transaction.getAll();

            request.onsuccess = () => {
                resolve(request.result);
            };

            request.onerror = _ => reject();
            return;
        }

        const result: unknown[] = [];
        const request = await startCursor(args.databaseName, args.currentVersion, args.objectStoreName);

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result;

            if (!cursor) {
                resolve(result);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                result.push(object);
            }
            cursor.continue();
        });
    });
}