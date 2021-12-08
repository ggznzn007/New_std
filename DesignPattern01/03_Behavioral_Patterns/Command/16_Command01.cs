/*Command Pattern 정의
  - 커맨드 패턴을 이용하면 요구 사항을 객체로 갭슐화 할 수 있으며, 매개변수를 써서 여러 가지 다른 요구 사항 추가가능,
    또한 요청 내역을 큐에 저장하거나 로그로 기록 할 수 있으며, 작업취소 기능도 지원이 가능하다.

사용용도 및 장점
  - 사용자의 요청을 객체화 시킴으로써 ( 객체 안에 작업에 필요한 모든 내용이 들어가있음 ), 그 객체만 있으면 해당 커맨드가 어떤
    작업을 수행했는지 알 수가있다. 그럼으로 해서 다시 취소작업을 수행한다거나 로그기능을 구현이 가능하다.
  - 커맨트 패턴은 커맨드에 알맞은 파라미터만 전달해주면 알아서 결과가 처리가된다. (예) 계산기
    요청만 하면 내부적으로 알고리즘에 의해서 자동으로 처리가 된다.*/

using System;
using System.Collections.Generic;

namespace Command
{
    class Program
    {
        static void Main(string[] args)
        {
            MapEditor mapEditor = new MapEditor();

            mapEditor.Create("마린1", "100", "200");
            mapEditor.Create("마린2", "200", "400");
            mapEditor.Create("마린3", "300", "600");
            mapEditor.Delete("마린2", "300", "600");

            mapEditor.Undo(3);
            mapEditor.Redo(3);

            Console.ReadKey();

        }
        /// <summary>
        /// Receiver 클래스
        /// </summary>
        class UnitManger
        {
            public void CreatUnit(string _unitName, string _pointX, string _pointY)
            {
                Console.WriteLine("[{0}] 유닛을 X좌표 {1} , Y좌표 {2} 위치에 생성합니다.", _unitName, _pointX, _pointY);
            }
            public void DeleteUnit(string _unitName, string _pointX, string _pointY)
            {
                Console.WriteLine("[{0}] 유닛을 X좌표 {1} , Y좌표 {2} 위치에서 삭제합니다.", _unitName, _pointX, _pointY);
            }
        }
        /// <summary>
        /// 추상  Command 생성
        /// </summary>
        abstract class Command
        {
            public string unitName;
            public string pointX;
            public string pointY;
            public abstract void Execute();
        }
        /// <summary>
        /// 유닛 생성 Concreate 객체 생성
        /// </summary>
        class CreatCommand : Command
        {
            private UnitManger unitManger;
            /// <summary>
            /// 생성자
            /// </summary>
            /// <param name="_unitManger"></param>
            public CreatCommand(UnitManger _unitManger, string _unitName, string _pointX, string _pointY)
            {
                this.unitManger = _unitManger;
                this.unitName = _unitName;
                this.pointX = _pointX;
                this.pointY = _pointY;
            }
            public override void Execute()
            {
                unitManger.CreatUnit(this.unitName, this.pointX, this.pointY);
            }
        }
        /// <summary>
        ///  유닛 삭제 Concreate 객체 생성
        /// </summary>
        class DeleteCommand : Command
        {
            private UnitManger unitManger;
            /// <summary>
            /// 생성자
            /// </summary>
            /// <param name="_unitManger"></param>
            public DeleteCommand(UnitManger _unitManger, string _unitName, string _pointX, string _pointY)
            {
                this.unitManger = _unitManger;
                this.unitName = _unitName;
                this.pointX = _pointX;
                this.pointY = _pointY;
            }
            public override void Execute()
            {
                unitManger.DeleteUnit(this.unitName, this.pointX, this.pointY);
            }
        }
        class MapEditor
        {
            private UnitManger _uintManger = new UnitManger();
            private List<Command> _commands = new List<Command>();
            private int _current = 0;
            public void Create(string _unitName, string _pointX, string _pointY)
            {
                Command command = new CreatCommand(_uintManger, _unitName, _pointX, _pointY);
                command.Execute();

                _commands.Add(command);
                _current++;
            }
            public void Delete(string _unitName, string _pointX, string _pointY)
            {
                Command command = new DeleteCommand(_uintManger, _unitName, _pointX, _pointY);
                command.Execute();

                _commands.Add(command);
                _current++;
            }
            public void Redo(int levels)
            {
                Console.WriteLine("\n---- Redo {0} levels ", levels);

                for (int i = 0; i < levels; i++)
                {
                    if (_current <= _commands.Count - 1)
                    {
                        Command command = _commands[_current++];
                        command.Execute();
                    }
                }
            }
            public void Undo(int levels)
            {
                Console.WriteLine("\n---- Undo {0} levels ", levels);
                // Perform undo operations
                for (int i = 0; i < levels; i++)
                {
                    if (_current > 0)
                    {
                        Command command = _commands[--_current] as Command;

                        if ((command as CreatCommand) != null)
                        {
                            //생성일때 삭제처리
                            Command commandTemp = new DeleteCommand(_uintManger, command.unitName, command.pointX, command.pointY);
                            commandTemp.Execute();
                        }
                        else
                        {
                            //삭제일때 생성처리
                            Command commandTemp = new CreatCommand(_uintManger, command.unitName, command.pointX, command.pointY);
                            commandTemp.Execute();
                        }
                    }
                }
            }
        }
    }
}