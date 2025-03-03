// @ts-ignore
import {processArguments} from "../ProcessArguments";
import {startQuery} from "../StartQuery";

export function single(json: string) : Promise<unknown> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);
        const request = await startQuery(args.databaseName, args.currentVersion, args.objectStoreName);
        let found = false;
        let result = null;

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
            }

            cursor.continue();
        });
    });
}