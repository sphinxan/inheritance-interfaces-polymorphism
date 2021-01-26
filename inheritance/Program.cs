using System;
using System.Collections;

namespace inheritance
{
	class Program
	{
		//проблема - отсутствие наследования - повтор метода
		static void Main()
		{
			SortIntArray(new int[] { 1, 2, 3 });
			SortStringArray(new string[] { "A", "B", "C" });
		}
		static void SortIntArray(int[] array)
		{
			for (int i = array.Length - 1; i > 0; i--)
				for (int j = 1; j <= i; j++)
					if (array[j] < array[j - 1])
					{
						var temp = array[j];
						array[j] = array[j - 1];
						array[j - 1] = temp;
					}
		}
		static void SortStringArray(string[] array)
		{
			for (int i = array.Length - 1; i > 0; i--)
				for (int j = 1; j <= i; j++)
					if (array[j].CompareTo(array[j - 1]) < 0)
					{
						var temp = array[j];
						array[j] = array[j - 1];
						array[j - 1] = temp;
					}
		}
	}

	//наследование
	class Transport
	{
		public double Velocity; //скорость
		public double KilometerPrice; //цена за км
		public int Capacity; //вместимость в людях
	}
	class CombustionEngineTransport : Transport // это двоеточие обозначает наследование
	{
		public double EngineVolume; //объем двигателя
		public double EnginePower; //мощность двигателя
	}
	enum ControlType // тип привода авто
	{
		Forward,
		Backward
	}
	class Car : CombustionEngineTransport
	{
		public ControlType Control;
	}
	class Program2
	{
		public static void Main()
		{
			var c = new Car();
			c.Control = ControlType.Backward; //это собственное поле класса Car
			c.EnginePower = 100; // это поле унаследовано от CombustionEngineTransport
			c.Capacity = 4; // это поле унаследовано от Transport
		}
	}

	//Иерархия наследования(+см классы выше)
	class Program3
	{
		public static void Main()
		{
			var car = new Car();

			var carAsTransport = (Transport)car; //это upcast - здесь мы начинаем смотреть на автомобиль как на какое-то транспортное средство
			Transport carAsTransport1 = car; //можно писать так. upcast - безопасная процедура, поэтому, как и с конверсией типа ее можно не указывать явно

			var car1 = (Car)carAsTransport;  //это downcast - мы снова начинаем смотреть на автомобиль, как на автомобиль

			var elephant = new Transport();
			//Car wrongCar = (Car)elephant; /этот downcast выбросит InvalidCastException, слон - не автомобиль. мы не можем смотреть на произвольный транспорт, как на автомобиль

			//оператор is позволяет проверить, является ли объект типа Transport на самом деле автомобилем
			if (elephant is Car)
			{
				Console.WriteLine("Ok, elephant is car");
			}
		}
	}

	// метод, который печатает все, что угодно, через запятую
	class Program4
	{
		public static void Main()
		{
			Print(1, 2); // 1, 2
			Print("a", 'b'); // a, b
			Print(1, "a"); // 1, a 
			Print(true, "a", 1); // True, a, 1
		}
		public static void Print(params object[] a)
		{
			for (var i = 0; i < a.Length; i++)
			{
				if (i > 0)
					Console.Write(", ");
				Console.Write(a[i]);
			}
			Console.WriteLine();
		}
	}

	//Склейка массивов
	class Program5
    {
		public static void Main()
		{
			var ints = new[] { 1, 2 };
			var strings = new[] { "A", "B" };

			Print(Combine(ints, ints)); // 1 2 1 2
			Print(Combine(ints, ints, ints)); // 1 2 1 2 1 2
			Print(Combine(ints)); // 1 2
			Print(Combine()); //null
			Print(Combine(strings, strings)); // A B A B
			Print(Combine(ints, strings)); //null
		}
		static void Print(Array array)
		{
			if (array == null)
			{
				Console.WriteLine("null");
				return;
			}
			for (int i = 0; i < array.Length; i++)
				Console.Write("{0} ", array.GetValue(i));
			Console.WriteLine();
		}
		static Array Combine(params Array[] arrays) //возвращает массив, собранный из переданных массивов
		{
			if (arrays.Length == 0)
				return null;
			var type = arrays[0].GetType().GetElementType(); //узнать тип элементов в переданном массиве, array.GetType().GetElementType()
			var length = 0;
			foreach (var array in arrays)
			{
				if (array.GetType().GetElementType() != type)
					return null;
				length += array.Length;
			}

			var result = Array.CreateInstance(type, length); //метод Array.CreateInstance принимает тип элемента массива
			var index = 0;
			foreach (var array in arrays)
				foreach (var elem in array)
					result.SetValue(elem, index++);
			return result;
		}
	}



	//Интерфейсы
	class Program6
    {
		public static void Sort(Array array)
		{
			for (int i = array.Length - 1; i > 0; i--)
				for (int j = 1; j <= i; j++)
				{
					object element1 = array.GetValue(j - 1);
					object element2 = array.GetValue(j);
					var comparableElement1 = (IComparable)element1; //IComparable - имя интерфейса
					//IComparable = int CompareTo(object obj) - сравинвает объект с другим и выводит -1 если другой меньше текущего(obj) и +1 другой больше obj и 0 если равны
					if (comparableElement1.CompareTo(element2) < 0)
					{
						object temporary = array.GetValue(j);
						array.SetValue(array.GetValue(j - 1), j);
						array.SetValue(temporary, j - 1);
					}
				}
		}
		static void Main()
		{
			Sort(new int[] { 1, 2, 3 });
			Sort(new string[] { "A", "B", "C" });
		}

		//среднее из трех через интерфейс
		public static void Main2()
		{
			Console.WriteLine(MiddleOfThree(2, 5, 4));
			Console.WriteLine(MiddleOfThree(3, 1, 2));
			Console.WriteLine(MiddleOfThree(3, 5, 9));
			Console.WriteLine(MiddleOfThree("B", "Z", "A"));
			Console.WriteLine(MiddleOfThree(3.45, 2.67, 3.12));
		}
		static IComparable MiddleOfThree(params IComparable[] array)
		{
			Array.Sort(array);
			return array[1];
		}


		//Реализация IComparable
		public class Point : IComparable
		{
			public double X;
			public double Y;

			public int CompareTo(object obj)
			{
				var point = (Point)obj;
				var thisDistance = Math.Sqrt(X * X + Y * Y);
				var thatDistance = Math.Sqrt(point.X * point.X + point.Y * point.Y);
				return thisDistance.CompareTo(thatDistance);
				//или if (thisDistance < thatDistance) return -1;
				//else if (thisDistance == thatDistance) return 0;
				//else return 1;
			}
		}

		// метод Min, который вычисляет минимум из элементов массива
		public static void Main3()
		{
			Console.WriteLine(Min(new[] { 3, 6, 2, 4 }));
			Console.WriteLine(Min(new[] { "B", "A", "C", "D" }));
			Console.WriteLine(Min(new[] { '4', '2', '7' }));
		}
		static object Min(Array array)
		{
			Array.Sort(array);
			return array.GetValue(0);
		}


		//сортировка книг, их сравнение
		class Book : IComparable
		{
			public string Title; //название
			public int Theme; //номер тематического раздела
			public int CompareTo(object obj)
			{
				var book = obj as Book;
				if (Theme.CompareTo(book.Theme) == 0)
					return Title.CompareTo(book.Title);
				else return Theme.CompareTo(book.Theme);
			}
		}

		//Интерфейс IComparer
		public class Point2
		{
			public double X;
			public double Y;
		}
		class DistanceToZeroComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				var point1 = (Point)x;
				var point2 = (Point)y;
				return
					Math.Sqrt(point1.X * point1.X + point1.Y * point1.Y).CompareTo(
					Math.Sqrt(point2.X * point2.X + point2.Y * point2.Y));
			}
		}
		class XDecreasingComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				var point1 = x as Point;
				var point2 = (Point)y;
				return -point1.X.CompareTo(point2.X);
			}
		}
		public class Program
		{
			public static void Sort(Array array, IComparer comparer)
			{
				for (int i = array.Length - 1; i > 0; i--)
					for (int j = 1; j <= i; j++)
					{
						object element1 = array.GetValue(j - 1);
						object element2 = array.GetValue(j);
						if (comparer.Compare(element1, element2) < 0)
						{
							object temporary = array.GetValue(j);
							array.SetValue(array.GetValue(j - 1), j);
							array.SetValue(temporary, j - 1);
						}
					}
			}
			public static void Main5()
			{
				var array = new[]
				{
					new Point { X=1, Y=1},
					new Point { X=2, Y=2}
				};
				Sort(array, new DistanceToZeroComparer());
				Sort(array, new XDecreasingComparer());
			}
		}

		//отсортировать точки в порядке следования против часовой стрелки, считая первой ту, что находится на 3:00
		private static void Main6()
		{
			var array = new[]
			{
				new Point { X = 1, Y = 0 },
				new Point { X = -1, Y = 0 },
				new Point { X = 0, Y = 1 },
				new Point { X = 0, Y = -1 },
				new Point { X = 0.01, Y = 1 }
			};
			Array.Sort(array, new ClockwiseComparer());
			foreach (Point e in array)
				Console.WriteLine("{0} {1}", e.X, e.Y);
		}
		public class Point3
		{
			public double X;
			public double Y;
		}
		public class ClockwiseComparer : IComparer
		{
			double GetAngle(Point point)
			{
				var angle = Math.Atan2(point.Y, point.X);
				if (angle < 0) angle += 2 * Math.PI;
				return angle;
			}
			public int Compare(object x, object y)
			{
				return GetAngle((Point)x).CompareTo(GetAngle((Point)y));
			}
		}

		//полиморфизм, Виртуальные методы
		class Point4
		{
			public int X;
			public int Y;

			public override string ToString() //сам метод
			{
				return string.Format("{0},{1}", X, Y);
			}
		}
		public class Program7 //тоже самое другими словами
		{
			static void Main()
			{
				var point = new Point { X = 1, Y = 3 };
				Console.WriteLine(point);
			}
		}

		//класс Triangle переопределяет метод ToString
		static void Main8()
		{
			var triangle = new Triangle
			{
				A = new Point { X = 0, Y = 0 },
				B = new Point { X = 1, Y = 2 },
				C = new Point { X = 3, Y = 2 }
			};
			Console.WriteLine(triangle.ToString());
		}
		public class Point5
		{
			public double X;
			public double Y;
			public override string ToString()
			{
				return string.Format("{0} {1}", X, Y);
			}
		}
		class Triangle
		{
			public Point A, B, C;
			public override string ToString()
			{
				return string.Format("({0}) ({1}) ({2})", A, B, C);
			}
		}

	}
}
