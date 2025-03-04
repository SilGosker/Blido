// @ts-ignore
import {processArguments} from "../ProcessArguments";
import {startQuery} from "../StartQuery";

export function singleOrDefault(json: string) : Promise<unknown> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);
        const request = await startQuery(args.databaseName, args.currentVersion, args.objectStoreName);
        let result : unknown | null = null;

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            const cursor = request.result as IDBCursorWithValue;

            if (!cursor) {
                resolve(result);
                return;
            }

            const object = cursor.value;
            if (args.matches(object)) {
                if (result) {
                    reject('More than one element satisfies the condition in predicate');
                    return;
                }
                result = object;
            }

            cursor.continue();
        });
    });
}