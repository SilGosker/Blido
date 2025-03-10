// @ts-ignore
import {processArguments} from "../ProcessArguments";
import {startTransaction} from "../StartCursor";

export function find(json: string) : Promise<number> {
    return new Promise(async (resolve, reject) => {
        const args = processArguments(json);

        const request = (await startTransaction(args.databaseName, args.currentVersion, args.objectStoreName))
            .get(args.id as any);

        request.addEventListener('error', reject);
        request.addEventListener('success', () => {
            resolve(request.result);
        });
    });
}