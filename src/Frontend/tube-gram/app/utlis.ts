export function isEmptyOrSpaces(str: string): boolean {
    return str === null || str.match(/^ *$/) !== null;
}