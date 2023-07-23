const BASE_64_PREFIX = 'base64,';

export class StringUtilities {
    
    public static removeBase64Prefix(str: string): string {
        // remove the data:image if it exists
        if (str.indexOf(BASE_64_PREFIX) > -1) {
            str = str.split(BASE_64_PREFIX)[1];
        }

        return str;
    }
}