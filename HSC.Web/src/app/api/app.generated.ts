/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.9.0 (NJsonSchema v10.6.8.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming
// @ts-nocheck

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable({
    providedIn: 'root'
})
export class BettingService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    check(matchId: string | undefined, userName: string | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/Betting/Check?";
        if (matchId === null)
            throw new Error("The parameter 'matchId' cannot be null.");
        else if (matchId !== undefined)
            url_ += "matchId=" + encodeURIComponent("" + matchId) + "&";
        if (userName !== undefined && userName !== null)
            url_ += "userName=" + encodeURIComponent("" + userName) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCheck(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCheck(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processCheck(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(null as any);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(null as any);
    }

    callAsnyc(matchId: string | undefined, userName: string | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/Betting/CallAsnyc?";
        if (matchId === null)
            throw new Error("The parameter 'matchId' cannot be null.");
        else if (matchId !== undefined)
            url_ += "matchId=" + encodeURIComponent("" + matchId) + "&";
        if (userName !== undefined && userName !== null)
            url_ += "userName=" + encodeURIComponent("" + userName) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCallAsnyc(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCallAsnyc(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processCallAsnyc(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(null as any);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(null as any);
    }

    raise(matchId: string | undefined, userName: string | null | undefined, newAmount: number | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/Betting/Raise?";
        if (matchId === null)
            throw new Error("The parameter 'matchId' cannot be null.");
        else if (matchId !== undefined)
            url_ += "matchId=" + encodeURIComponent("" + matchId) + "&";
        if (userName !== undefined && userName !== null)
            url_ += "userName=" + encodeURIComponent("" + userName) + "&";
        if (newAmount === null)
            throw new Error("The parameter 'newAmount' cannot be null.");
        else if (newAmount !== undefined)
            url_ += "newAmount=" + encodeURIComponent("" + newAmount) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processRaise(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processRaise(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processRaise(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(null as any);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(null as any);
    }

    fold(matchId: string | undefined, userName: string | null | undefined): Observable<void> {
        let url_ = this.baseUrl + "/api/Betting/Fold?";
        if (matchId === null)
            throw new Error("The parameter 'matchId' cannot be null.");
        else if (matchId !== undefined)
            url_ += "matchId=" + encodeURIComponent("" + matchId) + "&";
        if (userName !== undefined && userName !== null)
            url_ += "userName=" + encodeURIComponent("" + userName) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processFold(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processFold(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processFold(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(null as any);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(null as any);
    }
}

@Injectable({
    providedIn: 'root'
})
export class MatchService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    getMatchStartingData(matchId: string): Observable<MatchStartDto> {
        let url_ = this.baseUrl + "/api/Match/GetMatchStartingData/{matchId}";
        if (matchId === undefined || matchId === null)
            throw new Error("The parameter 'matchId' must be defined.");
        url_ = url_.replace("{matchId}", encodeURIComponent("" + matchId));
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetMatchStartingData(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetMatchStartingData(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<MatchStartDto>;
                }
            } else
                return _observableThrow(response_) as any as Observable<MatchStartDto>;
        }));
    }

    protected processGetMatchStartingData(response: HttpResponseBase): Observable<MatchStartDto> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = MatchStartDto.fromJS(resultData200);
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<MatchStartDto>(null as any);
    }
}

@Injectable({
    providedIn: 'root'
})
export class MatchFinderService {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    searchForMatch(dto: SearchingForMatchDto): Observable<void> {
        let url_ = this.baseUrl + "/api/MatchFinder/SearchForMatch";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(dto);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processSearchForMatch(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processSearchForMatch(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processSearchForMatch(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(null as any);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(null as any);
    }

    getCustomGames(): Observable<CustomGameDto[]> {
        let url_ = this.baseUrl + "/api/MatchFinder/GetCustomGames";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processGetCustomGames(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processGetCustomGames(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<CustomGameDto[]>;
                }
            } else
                return _observableThrow(response_) as any as Observable<CustomGameDto[]>;
        }));
    }

    protected processGetCustomGames(response: HttpResponseBase): Observable<CustomGameDto[]> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(CustomGameDto.fromJS(item));
            }
            else {
                result200 = <any>null;
            }
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<CustomGameDto[]>(null as any);
    }

    createCustomGame(dto: CreateCustomGameDto): Observable<void> {
        let url_ = this.baseUrl + "/api/MatchFinder/CreateCustomGame";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(dto);

        let options_ : any = {
            body: content_,
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Content-Type": "application/json",
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processCreateCustomGame(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processCreateCustomGame(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<void>;
                }
            } else
                return _observableThrow(response_) as any as Observable<void>;
        }));
    }

    protected processCreateCustomGame(response: HttpResponseBase): Observable<void> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return _observableOf<void>(null as any);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<void>(null as any);
    }

    joinCustomGame(challengeId: number | undefined): Observable<string> {
        let url_ = this.baseUrl + "/api/MatchFinder/JoinCustomGame?";
        if (challengeId === null)
            throw new Error("The parameter 'challengeId' cannot be null.");
        else if (challengeId !== undefined)
            url_ += "challengeId=" + encodeURIComponent("" + challengeId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ : any = {
            observe: "response",
            responseType: "blob",
            headers: new HttpHeaders({
                "Accept": "application/json"
            })
        };

        return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_ : any) => {
            return this.processJoinCustomGame(response_);
        })).pipe(_observableCatch((response_: any) => {
            if (response_ instanceof HttpResponseBase) {
                try {
                    return this.processJoinCustomGame(response_ as any);
                } catch (e) {
                    return _observableThrow(e) as any as Observable<string>;
                }
            } else
                return _observableThrow(response_) as any as Observable<string>;
        }));
    }

    protected processJoinCustomGame(response: HttpResponseBase): Observable<string> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;

        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 !== undefined ? resultData200 : <any>null;
    
            return _observableOf(result200);
            }));
        } else if (status !== 200 && status !== 204) {
            return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf<string>(null as any);
    }
}

export class MatchStartDto implements IMatchStartDto {
    blackUserName?: string | undefined;
    blackRating?: string | undefined;
    whiteUserName?: string | undefined;
    whiteRating?: string | undefined;

    constructor(data?: IMatchStartDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.blackUserName = _data["blackUserName"];
            this.blackRating = _data["blackRating"];
            this.whiteUserName = _data["whiteUserName"];
            this.whiteRating = _data["whiteRating"];
        }
    }

    static fromJS(data: any): MatchStartDto {
        data = typeof data === 'object' ? data : {};
        let result = new MatchStartDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["blackUserName"] = this.blackUserName;
        data["blackRating"] = this.blackRating;
        data["whiteUserName"] = this.whiteUserName;
        data["whiteRating"] = this.whiteRating;
        return data;
    }
}

export interface IMatchStartDto {
    blackUserName?: string | undefined;
    blackRating?: string | undefined;
    whiteUserName?: string | undefined;
    whiteRating?: string | undefined;
}

export class SearchingForMatchDto implements ISearchingForMatchDto {
    userName?: string | undefined;
    timeLimitMinutes!: number;
    increment!: number;
    minimumBet!: number;
    maximumBet!: number;
    rating!: number;

    constructor(data?: ISearchingForMatchDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.userName = _data["userName"];
            this.timeLimitMinutes = _data["timeLimitMinutes"];
            this.increment = _data["increment"];
            this.minimumBet = _data["minimumBet"];
            this.maximumBet = _data["maximumBet"];
            this.rating = _data["rating"];
        }
    }

    static fromJS(data: any): SearchingForMatchDto {
        data = typeof data === 'object' ? data : {};
        let result = new SearchingForMatchDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userName"] = this.userName;
        data["timeLimitMinutes"] = this.timeLimitMinutes;
        data["increment"] = this.increment;
        data["minimumBet"] = this.minimumBet;
        data["maximumBet"] = this.maximumBet;
        data["rating"] = this.rating;
        return data;
    }
}

export interface ISearchingForMatchDto {
    userName?: string | undefined;
    timeLimitMinutes: number;
    increment: number;
    minimumBet: number;
    maximumBet: number;
    rating: number;
}

export class CustomGameDto implements ICustomGameDto {
    challengeId!: number;
    userName?: string | undefined;
    rating!: number;
    timeLimitMinutes!: number;
    increment!: number;
    minimumBet!: number;
    maximumBet!: number;

    constructor(data?: ICustomGameDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.challengeId = _data["challengeId"];
            this.userName = _data["userName"];
            this.rating = _data["rating"];
            this.timeLimitMinutes = _data["timeLimitMinutes"];
            this.increment = _data["increment"];
            this.minimumBet = _data["minimumBet"];
            this.maximumBet = _data["maximumBet"];
        }
    }

    static fromJS(data: any): CustomGameDto {
        data = typeof data === 'object' ? data : {};
        let result = new CustomGameDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["challengeId"] = this.challengeId;
        data["userName"] = this.userName;
        data["rating"] = this.rating;
        data["timeLimitMinutes"] = this.timeLimitMinutes;
        data["increment"] = this.increment;
        data["minimumBet"] = this.minimumBet;
        data["maximumBet"] = this.maximumBet;
        return data;
    }
}

export interface ICustomGameDto {
    challengeId: number;
    userName?: string | undefined;
    rating: number;
    timeLimitMinutes: number;
    increment: number;
    minimumBet: number;
    maximumBet: number;
}

export class CreateCustomGameDto implements ICreateCustomGameDto {
    userName?: string | undefined;
    timeLimitMinutes!: number;
    increment!: number;
    minimumBet!: number;
    maximumBet!: number;

    constructor(data?: ICreateCustomGameDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.userName = _data["userName"];
            this.timeLimitMinutes = _data["timeLimitMinutes"];
            this.increment = _data["increment"];
            this.minimumBet = _data["minimumBet"];
            this.maximumBet = _data["maximumBet"];
        }
    }

    static fromJS(data: any): CreateCustomGameDto {
        data = typeof data === 'object' ? data : {};
        let result = new CreateCustomGameDto();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userName"] = this.userName;
        data["timeLimitMinutes"] = this.timeLimitMinutes;
        data["increment"] = this.increment;
        data["minimumBet"] = this.minimumBet;
        data["maximumBet"] = this.maximumBet;
        return data;
    }
}

export interface ICreateCustomGameDto {
    userName?: string | undefined;
    timeLimitMinutes: number;
    increment: number;
    minimumBet: number;
    maximumBet: number;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new ApiException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = event => {
                observer.next((event.target as any).result);
                observer.complete();
            };
            reader.readAsText(blob);
        }
    });
}