// Write your Javascript code.

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
                    h("input", { className: "form-control", onInput: e => props.parseType(e.target.value) })
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
            type: { typeName: "Object", value: { "": { typeName: "String", value: null, required: true } } },
            stringType: ""

        }
    }
    tryParseString(string) {
        try {
            var json = JSON.parse(string);
            console.log(JSON.stringify(json));
            Object.keys(json).forEach(jsonObject => {
                
            });
        } catch (err) {
            console.log(err.message);
        }
    }
    returnValue(json) {

    }

    tryParseObject(object) {
        try {
            
        } catch (err) {
            console.log(err);
        }
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
                h(RawInput, { type: state.type, parseType: string => this.tryParseString(string)})
                :
                h(ObjectInput, { type: state.type, first: true, onType: type => { console.log(type); this.setState({ type }) } })
        ]
        )
    }
}


preact.render(h(App, {}), document.getElementById("json-input"));