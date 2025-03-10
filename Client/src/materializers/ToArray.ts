// @ts-ignore
import {processArguments} from "../ProcessArguments";
import {startCursor} from "../StartCursor";

export function toArray(json: string) : Promise<unknown[]> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);

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