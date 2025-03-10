import {firstOrDefault} from "./materializers/FirstOrDefault";
import {getVersion} from "./GetVersion";
import {all} from "./materializers/All";
import {any} from "./materializers/Any";
import {average} from "./materializers/Average";
import {count} from "./materializers/Count";
import {first} from "./materializers/First";
import {last} from "./materializers/Last";
import {lastOrDefault} from "./materializers/LastOrDefault";
import {max} from "./materializers/Max";
import {min} from "./materializers/Min";
import {single} from "./materializers/Single";
import {singleOrDefault} from "./materializers/SingleOrDefault";
import {sum} from "./materializers/Sum";
import {toArray} from "./materializers/ToArray";
import {find} from "./materializers/Find";

class BlidoContext {
    private getVersion: (database: string) => Promise<number>;
    private all: (json: string) => Promise<boolean>;
    private any: (json: string) => Promise<boolean>;
    private average: (json: string) => Promise<number>;
    private count: (json: string) => Promise<number>;
    private first: (json: string) => Promise<unknown>;
    private firstOrDefault: (json: string) => Promise<unknown>;
    private last: (json: string) => Promise<unknown>;
    private lastOrDefault: (json: string) => Promise<unknown>;
    private max: (json: string) => Promise<number>;
    private min: (json: string) => Promise<number>;
    private single: (json: string) => Promise<unknown>;
    private singleOrDefault: (json: string) => Promise<unknown>;
    private sum: (json: string) => Promise<unknown>;
    private toArray: (json: string) => Promise<unknown[]>;
    private find: (json: string) => Promise<unknown>;
    public constructor() {
        this.firstOrDefault = firstOrDefault;
        this.getVersion = getVersion;
        this.all = all;
        this.any = any;
        this.average = average;
        this.count = count;
        this.first = first;
        this.last = last;
        this.lastOrDefault = lastOrDefault;
        this.max = max;
        this.min = min;
        this.single = single;
        this.singleOrDefault = singleOrDefault;
        this.sum = sum;
        this.toArray = toArray;
        this.find = find;
    }
}

(window as any).blido = new BlidoContext();