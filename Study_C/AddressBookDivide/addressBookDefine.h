/*
헤더파일이 여기 저기 #include되다보면
에러가 발생하게 된다.
그런데 아래처럼 처리하게 되면
중복이 방지되어 에러가 발생하지 않는다.
#ifndef __ADDRESSBOOKDEFINE_H__
#define __ADDRESSBOOKDEFINE_H__
#endif
*/
// Visual C++에서 제공하는 중복방지 옵션이다
#pragma once

#define ADDR_NUM	100

enum {
	INPUT_ADDR = 1,
	SEARCH_ADDR,
	UPDATE_ADDR,
	DELETE_ADDR,
	PRINTALL_ADDR,
	EXIT_ADDR
};

typedef struct _AddressBook
{
	char name[20];
	char address[30];
	int age;
	float height;
	float weight;
}AddressBook;
