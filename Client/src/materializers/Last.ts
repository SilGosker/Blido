import {processQueryArguments} from "../ProcessQueryArguments";
import {startCursor} from "../StartCursor";

export function last(json: string) : Promise<unknown> {
    return new Promise(async (resolve, reject) => {
        const args = processQueryArguments(json);
        const request = await startCursor(args.databaseName, args.currentVersion, args.objectStoreName);
        let last: unknown | null = null;

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                if (!last) {
                    reject('No element satisfies the condition in predicate');
                    return;
                }
                resolve(last);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                last = object;
            }

            cursor.continue();
        });
    });
}