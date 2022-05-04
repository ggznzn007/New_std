import random as rd # 랜덤 불러오기
print('\n')
print('\n\t\t UP & Down 게임')

choice = rd.randrange(100) # 0 ~ 99까지 랜덤한 숫자를 변수로 저장

while True:

    user_choice = int(input('\n\t100보다 작은 숫자를 입력하세요.  '))

    if choice == user_choice:
        break
    if choice < user_choice:
        print('\n\t Down !!!\n')
    else :
        print('\n\t Up !!!\n')


print('\n\t 정답입니다. 게임을 종료합니다.!!!\n')


