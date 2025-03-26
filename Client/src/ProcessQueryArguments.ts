export function processQueryArguments(json: string): QueryArguments {
    const parsed = JSON.parse(json) as
        { parsedSelector: string,
            database: string,
            objectStore: string,
            version: number,
            parsedExpressions: string[] | undefined,
            identifiers: IDBValidKey | undefined
        };

    let matches = (_: unknown) => true;
    const hasFilters = !!(parsed.parsedExpressions && parsed.parsedExpressions.length);
    if (hasFilters) {
        const functions = (parsed.parsedExpressions as string[]).map(x => eval(x));
        matches = (entity: unknown) => functions.every(x => x(entity));
    }

    let selector = undefined;
    if (parsed.parsedSelector) {
        selector = eval(parsed.parsedSelector);
    }

    return {
        databaseName: parsed.database,
        currentVersion: parsed.version,
        matches,
        objectStoreName: parsed.objectStore,
        selector,
        hasFilters,
        id: parsed.identifiers
    }
}

export interface QueryArguments {
    databaseName: string,
    objectStoreName: string,
    currentVersion: number,
    matches: (entity: unknown) => boolean;
    hasFilters: boolean
    selector: (entity: unknown) => number | unknown;
    id: IDBValidKey | undefined;
}