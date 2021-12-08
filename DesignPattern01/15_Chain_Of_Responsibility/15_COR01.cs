/*책임 연쇄 패턴 - 클래스 안에 연결 리스트 알고리즘을 걸고, 특정 함수를 실행하면 연속적으로 실행하는 패턴
             - 로그 처리나 하나의 처리로 여러가지 결과를 동시에 만들어야 할 때 사용하는 패턴*/

using System;
// Logger 추상 클래스
abstract class ALogger
{
    // 다음 Logger 포인터 프로퍼티
    protected ALogger Next
    {
        get; private set;
    }
    // 다음 포인터 Logger 설정
    public ALogger SetNextLogger(ALogger logger)
    {
        // 프로퍼티에 인스턴스 설정
        Next = logger;
        // 인스턴스 리턴
        return logger;
    }
    // 다음 인스턴스에 Write 함수 호출
    public virtual void Write(string data)
    {
        // 다음 포인터가 null 아니면
        if (Next != null)
        {
            // Write 함수 실행
            Next.Write(data);
        }
    }
}
// ConsoleLogger 클래스
class ConsoleLogger : ALogger
{
    // 함수 재정의
    public override void Write(string data)
    {
        // 콘솔 출력
        Console.WriteLine("ConsoleLogger - " + data);
        // 다음 포인터의 인스턴스의 함수 호출
        base.Write(data);
    }
}
// FileLogger 클래스
class FileLogger : ALogger
{
    // 함수 재정의
    public override void Write(string data)
    {
        // 콘솔 출력
        Console.WriteLine("FileLogger - " + data);
        // 다음 포인터의 인스턴스의 함수 호출
        base.Write(data);
    }
}
// MailLogger 클래스
class MailLogger : ALogger
{
    // 함수 재정의
    public override void Write(string data)
    {
        // 콘솔 출력
        Console.WriteLine("MailLogger - " + data);
        // 다음 포인터의 인스턴스의 함수 호출
        base.Write(data);
    }
}


// 실행 클래스
class Program
{
    // 실행 함수
    static void Main(string[] args)
    {
        // ConsoleLogger 인스턴스 생성
        var logger = new ConsoleLogger();
        // SetNextLogger 함수로 순서대로 FileLogger 인스턴스와 MailLogger 인스턴스 추가
        logger.SetNextLogger(new FileLogger())
              .SetNextLogger(new MailLogger());
        // 로그 작성
        logger.Write("Hello world");
        // 아무 키나 누르면 종료
        Console.WriteLine("Press Any key...");
        Console.ReadLine();
    }
}




