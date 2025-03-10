export function processArguments(json: string): Arguments {
    const parsed = JSON.parse(json) as
        { parsedSelector: string, database: string, objectStore: string, version: number, parsedExpressions: string[] | undefined, identifiers: object };

    let matches = (_: unknown) => true;
    if (parsed.parsedExpressions && parsed.parsedExpressions.length) {
        const functions = parsed.parsedExpressions.map(x => eval(x));
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
        id: parsed.identifiers
    }
}

export interface Arguments {
    databaseName: string,
    objectStoreName: string,
    currentVersion: number,
    matches: (entity: unknown) => boolean;
    selector: (entity: unknown) => number | unknown;
    id: string | number | object | null;
}