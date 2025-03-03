export function processArguments(json: string): Arguments {
    const parsed = JSON.parse(json) as
        { selector: string, database: string, objectStore: string, version: number, parsedExpressions: string[] | undefined };
    let matches = (_: unknown) => true;
    if (parsed.parsedExpressions && parsed.parsedExpressions.length) {
        const functions = parsed.parsedExpressions.map(x => eval(x));
        matches = (entity: unknown) => functions.every(x => x(entity));
    }

    let selector = undefined;
    if (parsed.selector) {
        selector = eval(parsed.selector);
    }

    return {
        databaseName: parsed.database,
        currentVersion: parsed.version,
        matches,
        objectStoreName: parsed.objectStore,
        selector
    }
}

export interface Arguments {
    databaseName: string,
    objectStoreName: string,
    currentVersion: number,
    matches: (entity: unknown) => boolean;
    selector: (entity: unknown) => number | unknown;
}