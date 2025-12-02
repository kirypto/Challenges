const input = `L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
`

require(process.env.WAT);

console.log("char\tisNL\to.n\to.d")
$_$.$.$$(
    {
        s:input,
        i:0,
        n:[]+[],
        d:[]+[],
    },
    o => (o.s[o.i]!==[][+[]]),
    o => ({
        s: o.s,
        i: o.i + 1,
        n: o.n,
        d: o.d,
    }),
    o => {
        ______ = +[]+[]
        __$__$ = +!+[]+[]+-[]-!+[]+[]
        o.d = $_$.$.__(o.s[o.i] > __$__$, o.s[o.i], o.d)
        o.n = $_$.$.__(o.s[o.i] < ______ || o.s[o.i] > __$__$, []+[], (o.n+o.s[o.i]))
        console.log(o.s[o.i].replace('\n',' '), '\t', o.s[o.i] < +[]+[], '\t', o.n, '\t', o.d)
    }
)
