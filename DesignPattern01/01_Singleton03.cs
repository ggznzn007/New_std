using System;

namespace Singleton03
{
    public sealed class Singleton03
    {
        //private 생성자 
        private Singleton03() { }
        //private static 인스턴스 객체
        private static readonly Lazy<Singleton03> _instance = new Lazy<Singleton03>(() => new Singleton03());
        //public static 의 객체반환 함수
        public static Singleton03 Instance { get { return _instance.Value; } }
    }
}