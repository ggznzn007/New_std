/*C# 비동기 델리게이트 (Asynchronous Delegate)

.NET의 비동기 델리게이트(Asynchronous delegate)는 쓰레드풀의 쓰레드를 사용하는 한 방식으로,
메서드 델리게이트(Delegate, 대리자)의 BeginInvoke()를 사용하여 쓰레드에게 작업을 시작하게 하고,
EndInvoke()를 사용하여 해당 작업이 끝날 때까지 기다려서 리턴 값을 넘겨 받게 된다.
BeginInvoke()는 쓰레드를 구동시킨 후 IAsyncResult 객체를 리턴하고 즉시 메인쓰레드의 다음 문장을 실행하게 된다.
IAsyncResult 객체는 차후 EndInvoke() 등과 같은 메서드를 실행할 때 파라미터로 전달되는 것으로서
어떤 쓰레드를 가리키는 지를 알 수 있게 한다.*/