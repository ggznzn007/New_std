using System;


/*C# Task<T> 클래스

Non-Generic 타입인 Task 클래스는 ThreadPool.QueueUserWorkItem()과 같이 리턴값을 쉽게 돌려 받지 못한다.
비동기 델리게이트(Asynchronous Delegate)와 같이 리턴값을 돌려 받기 위해서는 Task<T> 클래스를 사용한다.
Task<T> 클래스의 T는 리턴 타입을 가리키는 것으로 리턴값은 Task객체 생성 후 Result 속성을 참조해서 얻게 된다.
Result 속성을 참조할 때 만약 작업 쓰레드가 계속 실행 중이면, 결과가 나올 때까지 해당 쓰레드를 기다리게 된다.*/