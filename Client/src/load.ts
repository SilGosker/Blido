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
import {insert} from "./mutators/Insert";

export class BlidoContext {
    public getVersion: (database: string) => Promise<number>;
    public all: (json: string) => Promise<boolean>;
    public any: (json: string) => Promise<boolean>;
    public average: (json: string) => Promise<number>;
    public count: (json: string) => Promise<number>;
    public first: (json: string) => Promise<unknown>;
    public firstOrDefault: (json: string) => Promise<unknown>;
    public last: (json: string) => Promise<unknown>;
    public lastOrDefault: (json: string) => Promise<unknown>;
    public max: (json: string) => Promise<number>;
    public min: (json: string) => Promise<number>;
    public single: (json: string) => Promise<unknown>;
    public singleOrDefault: (json: string) => Promise<unknown>;
    public sum: (json: string) => Promise<unknown>;
    public toArray: (json: string) => Promise<unknown[]>;
    public find: (json: string) => Promise<unknown>;
    public   insert: (json: string) => Promise<unknown>;
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
        this.insert = insert;
    }
}

window.addEventListener("load", () => {
    Object.defineProperty(window, "blido", {
        value: new BlidoContext(),
        writable: false,
        configurable: false,
        enumerable: true
    });
})