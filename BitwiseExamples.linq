<Query Kind="Program" />

void Main()
{
	IsOdd(2).Dump();
}

public bool IsOdd(int x) => BitwiseAnd(x, 1) != 0;

//AND: Compares each bit and results in 1 if both bits are 1, otherwise 0.
public int BitwiseAnd(int a, int b) => a & b;

//OR: Compares each bit and results in 1 if at least one bit is 1.
public int BitwiseOr(int a, int b) => a | b;

//XOR: Compares each bit and results in 1 if bits are different.
public int BitwiseXor(int a, int b) => a ^ b;

//NOT: Inverts each bit (1 becomes 0, 0 becomes 1 etc).
public int BitwiseNot(int a) => ~a;

//Left Shift: Shifts the bits to the left by the specified number of positions, adding 0s on the right.
public int LeftShift(int a) => a << 1;

//Right Shift: Shifts the bits to the right by the specified number of positions, dropping bits on the right.
public int RightShift(int a) => a >> 1;