﻿/*
 *  随机数发生器，Unity和C#自带的生成器都是随机序列发生器，就是存入一个种子之后可以不断Next获取随机数（Unity虽然可以不存种子但不存种子的情况下Unity会自己存进去一个种子，所以还是要存入种子）
 *  
 *  自带发生器只要存入一个种子就能获取一大串的随机数十分方便，但如果要获取指定第几个随机数那就不好办了
 *  自带发生器的原理是把这一个随机数计算过程中的某个值作为下一个随机数的种子，也就是说获取第几个随机数并不能通过传第二个参数获取，必须要Next到那一个才能获取
 *  
 *  对于一般情况自带发生器只要传一次种子就能不断获取随机数非常方便，Unity的自动存种子更是方便到只要一个调用就能获取随机数
 *  可惜，这对柏林噪声而言并不是那么方便，柏林噪声每个值的计算都要先获取到指定的第几个随机数，用自带发生器就要疯狂Next，计算量严重浪费到完全不能承受
 *  
 *  这时候自己动手写一个随机数生成器就显得很重要了
 */


public static class RandomNumber
{
    /*
     *  单参数，不适用于柏林噪声
     *  
     *  测试场景用横向排列的点展示结果，用平均值线和平均数数字提示平均数
     *  
     *  需要达到以下几点要求：
     *      1.种子变化要引起结果的变化，达不到这条图像上所有点排成横线
     *      2.不能出现镜像，表现为在某个值左右的点形成了镜像的图像，很容易穿帮
     *      3.图像不能有明显的规律，无规律是随机数的重要要求
     *      4.平均，设计上这个方法返回 0-1 的值，则最好平均值在0.5左右
     *      5.均匀，图像要均匀，不能出现严重的堆积现象，注意绝对的均匀必然有明显的规律，无规律高于均匀
     *  
     *  计算和常数随便写，主要是对着图像改，一直改到满足上面的要求为止，结果就是这样了
     */
    public static float GetFloat(int seed)
    {
        int value = seed * 2233681 ^ seed;
        /*
         *  这一步有两个目的：
         *      1.相邻的种子只差了 1，靠这么小的差距计算出分布均匀的随机数操作难度实在大，那就先拉开值
         *      2.只靠种子一个值产生看似无规律的随机数也很困难，这里计算出第二个值后面计算轻松点
         */

        value = (value * (value - 155786) ^ seed) * (value * 1679542) + 736779151;      //这一步计算很明显会超出int的最大值，但要的就是这个效果，int超过最大值会溢出成负数，最大化利用int空间

        return IntToZeroToOneFloat(value);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
    }

    /*
     *  双种子，适用于1维柏林噪声的随机数发生器
     *  
     *  测试场景里用二维图像展示结果
     *  两个种子对图像的影响需要同时满足以下几点
     *      1.任何一个种子的变化都要造成结果的变化，如果这条达不到图像会特别有规律性或形成很大的纯色块
     *      2.不能发生一个种子到达某个值的时候另一个种子怎么改变都不会改变结果的情况，如果发生了图像上会看见一条线
     *      3.图像不能明显看出规律
     *      4.不能出现镜像，一般来说表现为在某个值的两边图像镜像，注意这个值可能发生在种子为负数的时候，需要把偏转值设为负数来查看
     *  
     *  各种常数和计算都是乱写 + 对着图像修修改改弄出来的，主要是修改
     */
    public static float GetFloat(int seedA, int seedB)
    {
        int valueA = (seedA * 16579324) * seedA - 1359874;
        int valueB = (seedB * 165972) * seedB - 459823;

        int value = (((seedA + 1322997815) * (seedB - 213549494)) * (valueA - 46218954) * valueA * valueB) + (((seedA - 14675237) * (seedB + 213746794)) * (valueB - 474331954) * valueA * valueB);

        return IntToZeroToOneFloat(value);
    }
    
    public static int GetInt(int seedA, int seedB)
    {
        int valueA = (seedA * 16579324) * seedA - 1359874;
        int valueB = (seedB * 165972) * seedB - 459823;

        int value = (((seedA + 1322997815) * (seedB - 213549494)) * (valueA - 46218954) * valueA * valueB) + (((seedA - 14675237) * (seedB + 213746794)) * (valueB - 474331954) * valueA * valueB);

        return value;
    }



    //三种子
    public static float GetFloat(int seedA, int seedB, int seedC)
    {
        int valueA = (seedA * 1664972) * seedA + 1497513149;
        int valueB = (seedB * 13344) * seedB - 1648713494;
        int valueC = (seedC * 116498) * seedC + 164879715;

        int value = (((seedA + 1654931) * valueA) + (seedB * (valueB - 16449823)) + ((seedC - 659873) * (valueC + 9871513))) + (valueA * valueB + 165478921) * (1987431 - valueC * valueB) * valueB * valueC + 16887423;

        return IntToZeroToOneFloat(value);
    }
    public static int GetInt(int seedA, int seedB, int seedC)
    {
        int valueA = (seedA * 1664972) * seedA + 1497513149;
        int valueB = (seedB * 13344) * seedB - 1648713494;
        int valueC = (seedC * 116498) * seedC + 164879715;

        int value = (((seedA + 1654931) * valueA) + (seedB * (valueB - 16449823)) + ((seedC - 659873) * (valueC + 9871513))) + (valueA * valueB + 165478921) * (1987431 - valueC * valueB) * valueB * valueC;

        return value;
    }




    static float IntToZeroToOneFloat(int intValue)
    {
        return (float)(intValue & int.MaxValue) / int.MaxValue;
    }
}
