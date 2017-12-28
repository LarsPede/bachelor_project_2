// Write your Javascript code.

var h = preact.h;
var Component = preact.Component

function ObjectInput({ type, first, onType }) {
    return h(
        "div",
        { className: "col-sm-12", style: first ? {paddingLeft: "0"} : { marginLeft: "50px" } },
        Object.keys(type.value).map((key, i, allKeys) => {
            return h(JsonDefinition, {
                keyName: key,
                type: type.value[key],
                canDelete: i !== 0,
                first: first,
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
                changeRequired: () => {
                    type.value[key].requiredKey = !type.value[key].requiredKey
                    onType(type);
                }
            })
        }).concat(
            h(
                "div",
                {
                    className: "btn btn-success",
                    onClick: () => {
                        type.value[""] = { typeName: "String", value: null, requiredKey: false }
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
        { className: "col-sm-12", style: { marginLeft: "290px"} },
        type.value.map((t, key) => {
            return h(JsonDefinition, {
                canDelete: key !== 0 && key === type.value.length-1,
                key,
                keyName: null,
                type: t,
                onType: subType => {
                    type.value[key] = subType;
                    onType(type);
                },
                onDelete: () => {
                    type.value.pop();
                    onType(type);
                }
            })
        }).concat(
            h(
                "div",
                {
                    className: "btn btn-success",
                    onClick: () => {
                        type.value.push({ typeName: "String", value: null, requiredKey: false })
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
                this.props.first ?
                    h("input", {
                        type: "checkbox", onClick: () => {
                            props.changeRequired();
                        }, checked: this.props.type.requiredKey, className: "pull-right"
                    })
                    :
                    h(),
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
            type: { typeName: "Object", value: { "id": { typeName: "String", value: null, requiredKey: true }} },
            stringType: "{ \"id\" : \"\" }",
            requiredKeys: "{ \"required\" : [ \"id\" ] }",
            sendType: "{ \"typeName\": \"Object\", \"value\": { \"id\": { \"typeName\": \"String\", \"value\": null, \"requiredKey\": true }} }"

        }
    }
    tryParseString(string) {
        try {
            let json = JSON.parse(string);
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
            var temp = JSON.parse(concatString);
            this.setState({ sendType: JSON.stringify(temp) });
            return temp;
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
                        concatString += " { \"typeName\": \"String\", \"value\": null, \"requiredKey\" : false}";
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
                    concatString += "}, \"requiredKey\" : false";
                }
            } else if (typeof (value) === "boolean") {
                concatString += " \"typeName\": \"Boolean\", \"value\": null, \"requiredKey\" : false";
            } else if ($.isNumeric(value)) {
                concatString += " \"typeName\": \"Number\", \"value\": null, \"requiredKey\" : false";
            } else if (value === "timestamp()") {
                concatString += " \"typeName\": \"Unix Timestamp\", \"value\": null, \"requiredKey\" : false";
            } else {
                concatString += " \"typeName\": \"String\", \"value\": null, \"requiredKey\" : false";
            }
            concatString += "}";
            return concatString;
            
        } catch (err) {
            
        }
    }

    tryParseReqiuredKeys(json) {
        try {
            let concatString = "{ \"required\": [";
            let requiredArray = [];
            if (json.value !== null) { 
                Object.keys(json.value).forEach(jsonKey => {
                    if (json.value[jsonKey].requiredKey === true) {
                        requiredArray.push(jsonKey);
                    }
                });
                requiredArray.forEach((arrayValue, arrayKey) => {
                    concatString += arrayValue;
                    if (arrayKey !== requiredArray.length - 1) {
                        concatString += ",";
                    }
                });
                concatString += "]}";
            }
            return concatString;
        } catch (err) {
            return this.state.type
        }
    }

    tryParseObject(json, array = false) {
        try {
            if (json.value !== null) {
                if (array) {
                    let concatString = "[";
                    if (json.value.length > 0) {
                        json.value.forEach((arrayValue, arrayKey) => {
                            switch (arrayValue.typeName) {
                                case "Number": {
                                    concatString += "0";
                                    break;
                                }
                                case "String": {
                                    concatString += "\"\"";
                                    break;
                                }
                                case "Boolean": {
                                    concatString += "true";
                                    break;
                                }
                                case "Unix Timestamp": {
                                    concatString += "\"timestamp()\"";
                                    break;
                                }
                                case "Object": {
                                    concatString += this.tryParseObject(arrayValue);
                                    break;
                                }
                                case "Array": {
                                    concatString += this.tryParseObject(arrayValue, true);
                                    break;
                                }
                            }
                            if (arrayKey !== json.value.length-1) {
                                concatString += ",";
                            }
                        });
                    } else {
                        concatString += "\"\"";
                    }
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
            } else {
                return "{}";
            }
        } catch (err) {
        }
    }

    render(props, state) {
        return h("div", {}, [
            h("input", { type: "hidden", id: "JsonContentAsString", name: "JsonContentAsString", value: state.sendType }),
            h("input", { type: "hidden", id: "JsonRequiredKeys", name: "JsonRequiredKeys", value: state.requiredKeys }),
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
                h()
                :
                h("span",
                    {
                        className: "lead pull-right"
                    },
                    "Required?"
                ),
            state.showRaw ?
                h(RawInput, {
                    stringType: state.stringType, type: state.type, parseType: string => {
                        this.setState({ stringType: string });
                        this.setState({ type: this.tryParseString(string) });
                        this.setState({ requiredKeys: "{\"required\" : []}" });
                    }
                })
                :
                h(ObjectInput, {
                    type: state.type, first: true, onType: type => {
                        this.setState({ stringType: this.tryParseObject(type) });
                        this.setState({ sendType: JSON.stringify(type) });
                        this.setState({ requiredKeys: this.tryParseReqiuredKeys(type) });
                        this.setState({ type });
                    }
                })
        ]
        )
    }
}


preact.render(h(App, {}), document.getElementById("json-input"));