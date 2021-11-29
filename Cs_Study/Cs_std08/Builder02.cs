public class ScoreBuilder
{
    private StudentScore studentScore;

    public ScoreBuilder()
    {
        studentScore = new StudentScore(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
    }

    public ScoreBuilder SetMat(int val)
    {
        studentScore.Mat = val;
        return this;
    }
    public ScoreBuilder SetEng(int val)
    {
        studentScore.Eng = val;
        return this;
    }
    public ScoreBuilder SetKor(int val)
    {
        studentScore.Kor = val;
        return this;
    }
    public ScoreBuilder SetSoc(int val)
    {
        studentScore.Soc = val;
        return this;
    }
    public ScoreBuilder SetSci(int val)
    {
        studentScore.Sci = val;
        return this;
    }
    public ScoreBuilder SetArt(int val)
    {
        studentScore.Art = val;
        return this;
    }
    public ScoreBuilder SetSki(int val)
    {
        studentScore.Ski = val;
        return this;
    }
    public ScoreBuilder SetAth(int val)
    {
        studentScore.Ath = val;
        return this;
    }
    public ScoreBuilder SetCom(int val)
    {
        studentScore.Com = val;
        return this;
    }
    public ScoreBuilder SetMus(int val)
    {
        studentScore.Mus = val;
        return this;
    }
}