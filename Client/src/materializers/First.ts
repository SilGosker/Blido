// @ts-ignore
import {processArguments} from "../ProcessArguments";
import {startQuery} from "../StartQuery";

export function first(json: string) : Promise<unknown[]> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);

        const request = await startQuery(args.databaseName, args.currentVersion, args.objectStoreName);

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                reject('No element satisfies the condition in predicate');
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                resolve(object);
                return;
            }

            cursor.continue();
        });
    });
}