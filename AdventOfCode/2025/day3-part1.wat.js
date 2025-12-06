$_$ = require(process.env.WAT);



$$$ = (_) => ((_$_) => ((__, __$, __$_, $$) => {
    _$_ = {
        $: 0
    }
    __$ = _[$_$.$.__$(_) - !-[]]  // Get '\n' (it's the last character of the input)
    __ = $_$.$.$__(_, __$)        // Split the input at '\n's

    __$ = $_$.$.__$(__)  // Get the number of input lines

    $_$.$.$$([] - [], $ => $ < __$, $ => $ + !+[], $ => { // For each line
        __$_ = $_$.$.__$(__[$])  // Get the length of the input line
        console.log(`Processing Line '${__[$]}'`)
        $$ = {
            __: [][[]],  // Placeholder for max char except last (10s digit)
            $$: [][[]],  // Placeholder for index of 10s digit
            _: [][[]],   // Placeholder for max char from 10s digit on
        }
        $$.__ = $_$.$.__$$($_$.$._$$_(__[$], []-[], __$_-!-[])) // Get the 10s digit
        $$.$$ = __[$].indexOf($$.__) // I need to write an indexOf method...
        $$._ = $_$.$.__$$($_$.$._$$_(__[$], $$.$$+!+[], __$_)) // Get the 1s digit
        console.log($$)
    })
})() || _$_.$)()

module.exports = $$$
