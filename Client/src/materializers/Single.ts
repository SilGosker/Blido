import {processQueryArguments} from "../ProcessQueryArguments";
import {startCursor} from "../StartCursor";

export function single(json: string) : Promise<unknown> {
    return new Promise(async (resolve, reject) => {
        const args = processQueryArguments(json);
        const request = await startCursor(args.databaseName, args.currentVersion, args.objectStoreName);
        let found = false;
        let result: unknown = null;

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                if (!result) {
                    reject('No element satisfies the condition in predicate');
                    return;
                }
                resolve(result);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                if (found) {
                    reject('More than one element satisfies the condition in predicate');
                    return;
                }
                found = true;
                result = object;
            }
            cursor.continue();
        });
    });
}