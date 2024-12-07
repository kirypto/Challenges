namespace kirypto.AdventOfCode.Common.Interfaces;

public interface IDailyProgram {
    public string Run(IInputRepository inputRepository, string inputRef, int part);
}
