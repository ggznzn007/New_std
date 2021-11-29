using System;

namespace DesignPattern01
{
    class Pro
    {
        public static void Main(string[] args)
        {
            ScoreBuilder scoreBuilder = new ScoreBuilder();
            scoreBuilder.SetMat(80).SetArt(100).SetAth(90).SetCom(70).SetEng(88).SetKor(87).SetMus(100).SetSci(79).SetSki(68).SetSoc(90);

            
        }
    }
}