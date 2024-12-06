namespace kirypto.AdventOfCode.Common.Interfaces;

public interface IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part);
}