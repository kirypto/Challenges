using kirypto.AdventOfCode.Common.AOC;

namespace kirypto.AdventOfCode.Common.Repositories;

public static class InputRepositoryFactory {
    public static IInputRepository CreateInputRepository(AocProgramArguments args) {
        return args.FetchCode is not null
                ? new RestInputRepository(args.Day, args.FetchCode)
                : new FileInputRepository(args.InputFile);
    }
}
