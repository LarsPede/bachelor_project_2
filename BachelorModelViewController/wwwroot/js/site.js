﻿// Write your Javascript code.

var h = preact.h;
var Component = preact.Component

class Counter extends Component {
    constructor(props) {
        super(props);
        this.state = {
            time: Date.now()
        };
    }

    componentDidMount() {
        setInterval(() => {
            this.setState({
                time: Date.now()
            });
        }, 1000);
    }

    render(props, state) {
        return state.time;
    }
}

function ObjectInput({ type, first, onType }) {
    return h(
        "div",
        { className: "col-sm-12", style: first ? {paddingLeft: "0"} : { marginLeft: "50px" } },
        Object.keys(type.value).map((key, i, allKeys) => {
            return h(JsonDefinition, {
                keyName: key,
                type: type.value[key],
                canDelete: i !== 0,
                onType: subType => {
                    type.value[key] = subType;
                    onType(type);
                },
                onKey: newKey => {
                    type.value[newKey] = type.value[key];
                    delete type.value[key];
                    onType(type);
                },
                onDelete: () => {
                    delete type.value[key];
                    onType(type);
                },
            })
        }).concat(
            h(
                "div",
                {
                    className: "btn btn-success",
                    onClick: () => {
                        type.value[""] = { typeName: "String", value: null }
                        onType(type)
                    },
                    style: { "margin-top": "10px" }
                },
                h(
                    "span",
                    { className: "glyphicon glyphicon-plus" }
                )
            )
        )
    )
}
function ArrayInput({ type, onType }) {
    return h(
        "div",
        { className: "col-sm-12", style: { marginLeft: "50px"} },
        type.value.map((t, key) => {
            return h(JsonDefinition, {
                canDelete: key !== 0,
                key,
                keyName: null,
                type: t,
                onType: subType => {
                    type.value[key] = subType;
                    onType(type);
                }
            })
        }).concat(
            h(
                "div",
                {
                    className: "btn btn-success",
                    onClick: () => {
                        type.value.push({ typeName: "String", value: null })
                        onType(type)
                    },
                    style: { "margin-top": "10px" }
                },
                h(
                    "span",
                    { className: "glyphicon glyphicon-plus" }
                )
            )
        )
    )
}

class JsonDefinition extends Component {
    renderSubField(type, onType) {
        switch (type.typeName) {
            case "Object": {
                return h(ObjectInput, { type, first: false, onType })
            }
            case "Array": {
                return h(ArrayInput, { type, onType })
            }
        }
    }
    render(props, state) {
        return h(
            "div",
            { className: "row" },
            null,
            [
                props.keyName !== null ?h(
                    "div",
                    { className: "col-sm-3" },
                    null,
                    h("input", { onChange: e => props.onKey(e.target.value), value: props.keyName, className: "form-control" })
                ): h(),
                h(
                    "div",
                    { className: "col-sm-3" },
                    null,
                    h(
                        "select",
                        {
                            className: "form-control", onChange: (e) => {
                                let value = null;
                                if (e.target.value == "Object") {
                                    value = { "": { typeName: "String" } }
                                }
                                if (e.target.value == "Array") {
                                    value = [{typeName: "String", value: null}]
                                }
                                props.onType({ typeName: e.target.value, value })
                            }
                        },
                        ["Number", "String", "Boolean", "Unix Timestamp", "Object", "Array"].map(typeName => {
                            return h("option", { value: typeName, selected: props.type.typeName === typeName }, typeName)
                        })
                    )
                ),
                h("span", null, state.type),
                this.renderSubField(props.type, props.onType),
                this.props.canDelete ?
                    h(
                        "div",
                        { className: "btn btn-danger",
                            onClick: () => props.onDelete(),
                            style: { "margin-top": "10px" }
                        },
                        h(
                            "span",
                            { className: "glyphicon glyphicon-minus" }
                        )
                    )
                    :
                    h(),
                this.props.notObjectOrArray ?
                    h("input",{type:"checkbox", className: "pull-right"})
                    :
                    h()
            ]
        )
    }
}
JsonDefinition.defaultProps = { canDelete: true, addBelow: false };

class RawInput extends Component {
    render(props, state) {
        return h(
            "div",
            { className: "row" },
            null,
            h(
                "div",
                { className: "col-sm-12" },
                null,
                [
                    h("input", { value: props.stringType, className: "form-control", onInput: e => props.parseType(e.target.value) })
                ]
            )
        )
    }
}

class App extends Component {
    constructor(props) {
        super(props);
        this.state = {
            showRaw: false,
            type: { typeName: "Object", value: { "": { typeName: "String", value: null } } },
            stringType: "{}"

        }
    }
    tryParseString(string) {
        try {
            let json = JSON.parse(string);
            console.log(JSON.stringify(json));
            let concatString = "{ \"typeName\": \"Object\", \"value\": {";
            var totalLength = Object.keys(json).length;
            var i = 0;
            Object.keys(json).forEach(jsonKey => {
                i++;
                concatString += this.returnValue(jsonKey, json[jsonKey], false);
                if (i !== totalLength) {
                    concatString += ",";
                }
            });
            concatString += "}}";
            return JSON.parse(concatString);
        } catch (err) {
            return this.state.type
        }
    }
    returnValue(key, value, array = false) {
        try {
            let concatString = '';
            if (array) {
                concatString += '{';
            } else {
                concatString += ' \"' + key + '\" : {';
            }
            if (typeof value === 'object') {
                if (Array.isArray(value)) {
                    concatString += "\"typeName\": \"Array\", \"value\": [";
                    if (value.length > 0) {
                        value.map((arrayValue, arrayKey) => {
                            concatString += this.returnValue(arrayKey, arrayValue, true);
                            if (arrayKey !== value.length - 1) {
                                concatString += ",";
                            }
                        });
                    } else {
                        concatString += " { \"typeName\": \"String\", \"value\": null}";
                    }
                    concatString += "]";
                } else {
                    concatString += " \"typeName\": \"Object\", \"value\" : {";
                    var totalLength = Object.keys(value).length;
                    var i = 0;
                    Object.keys(value).forEach(jsonKey => {
                        i++;
                        concatString += this.returnValue(jsonKey, value[jsonKey], false);
                        if (i !== totalLength) {
                            concatString += ",";
                        }
                    });
                    concatString += "}";
                }
            } else if (typeof (value) === "boolean") {
                concatString += " \"typeName\": \"Boolean\", \"value\": null";
            } else if ($.isNumeric(value)) {
                concatString += " \"typeName\": \"Number\", \"value\": null";
            } else if (value === "timestamp()") {
                concatString += " \"typeName\": \"Unix Timestamp\", \"value\": null";
            } else {
                concatString += " \"typeName\": \"String\", \"value\": null";
            }
            concatString += "}";
            return concatString;
            
        } catch (err) {
            
        }
    }

    tryParseObject(json, array = false) {
        try {
            if (array) {
                let concatString = "[";
                json.value.forEach((arrayValue, arrayKey) => {
                    switch (arrayValue.typeName) {
                        case "Number": {
                            concatString += "0";
                        }
                        case "String": {
                            concatString += "\"\"";
                        }
                        case "Boolean": {
                            concatString += "true";
                        }
                        case "Unix Timestamp": {
                            concatString += "\"timestamp()\"";
                        }
                        case "Object": {
                            concatString += this.tryParseObject(arrayValue);
                        }
                        case "Array": {
                            concatString += this.tryParseObject(arrayValue, true);
                        }
                    }
                    if (arrayKey !== json.value.length) {
                        concatString += ",";
                    }
                });
                concatString += "]"
                return concatString;
            } else {
                let concatString = "{";
                var totalLength = Object.keys(json.value).length;
                var i = 0;
                Object.keys(json.value).forEach(jsonKey => {
                    i++;
                    switch (json.value[jsonKey].typeName) {
                        case "Number": {
                            concatString += "\"" + jsonKey + "\": 0";
                            break;
                        }
                        case "String": {
                            concatString += "\"" + jsonKey + "\": \"\"";
                            break;
                        }
                        case "Boolean": {
                            concatString += "\"" + jsonKey + "\": true";
                            break;
                        }
                        case "Unix Timestamp": {
                            concatString += "\"" + jsonKey + "\": \"timestamp()\"";
                            break;
                        }
                        case "Object": {
                            concatString += "\"" + jsonKey + "\":" + this.tryParseObject(json.value[jsonKey]);
                            break;
                        }
                        case "Array": {
                            concatString += "\"" + jsonKey + "\":" + this.tryParseObject(json.value[jsonKey], true);
                            break;
                        }
                    }
                    if (i !== totalLength) {
                        concatString += ",";
                    }
                });
                concatString += "}"
                return concatString;
            }
            
        } catch (err) {
            console.log(err);
        }
    }

    returnAsString(json) {

    }

    render(props, state) {
        return h("div", {}, [
            h("button",
                {
                    type: "button",
                    onClick: () => {
                        this.setState({
                            showRaw: !state.showRaw
                        })
                    },
                    style: { "margin-bottom": "10px" }
                },
                state.showRaw ? "Describe Format." : "Let us parse a JSON Object for you."
            ),
            state.showRaw ?
                h(RawInput, {
                    stringType: state.stringType, type: state.type, parseType: string => {
                        this.setState({ stringType: string });
                        this.setState({ type: this.tryParseString(string) });
                    }
                })
                :
                h(ObjectInput, {
                    type: state.type, first: true, onType: type => {
                        this.setState({ stringType: this.tryParseObject(type) });
                        this.setState({ type });
                    }
                })
        ]
        )
    }
}


preact.render(h(App, {}), document.getElementById("json-input"));