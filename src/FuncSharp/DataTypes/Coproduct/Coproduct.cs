﻿using System;

namespace FuncSharp
{
    /// <summary>
    /// Base class and factory of canonical coproduct types.
    /// </summary>
    public abstract class CoproductBase : ICoproduct
    {
        public CoproductBase(int arity, int discriminator, object value)
        {
            if (arity <= 0)
            {
                throw new ArgumentException("The arity must be a positive number.");
            }
            if (discriminator < 1 || arity < discriminator)
            {
                throw new ArgumentException("The discriminator must be from interval [1, arity].");
            }

            CoproductArity = arity;
            CoproductDiscriminator = discriminator;
            CoproductValue = value;
        }

        public int CoproductArity { get; }

        public int CoproductDiscriminator { get; }

        public object CoproductValue { get; }

        public override int GetHashCode()
        {
            return this.CoproductHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.CoproductEquals(obj);
        }

        public override string ToString()
        {
            return this.CoproductToString();
        }

		protected T GetCoproductValue<T>()
        {
            if (CoproductValue is T)
            {
                return (T)CoproductValue;
            }
            return default(T);
        }
    }

    /// <summary>
    /// A 0-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct0 : CoproductBase, ICoproduct0
    {
        protected Coproduct0()
            : base(0, 0, null)
        {
        }
    }

    /// <summary>
    /// Factory for 1-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct1
    {
        /// <summary>
        /// Creates a new 1-dimensional coproduct with the first value.
        /// </summary>
        public static ICoproduct1<T1> CreateFirst<T1>(T1 value)
        {
            return new Coproduct1<T1>(value);
        }

    }

    /// <summary>
    /// A 1-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct1<T1> : CoproductBase, ICoproduct1<T1>
    {
        /// <summary>
        /// Creates a new 1-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct1(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 1-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct1(ICoproduct1<T1> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 1-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct1(int discriminator, object value)
            : base(1, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 2-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct2
    {
        /// <summary>
        /// Creates a new 2-dimensional coproduct with the first value.
        /// </summary>
        public static ICoproduct2<T1, T2> CreateFirst<T1, T2>(T1 value)
        {
            return new Coproduct2<T1, T2>(value);
        }

        /// <summary>
        /// Creates a new 2-dimensional coproduct with the second value.
        /// </summary>
        public static ICoproduct2<T1, T2> CreateSecond<T1, T2>(T2 value)
        {
            return new Coproduct2<T1, T2>(value);
        }

    }

    /// <summary>
    /// A 2-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct2<T1, T2> : CoproductBase, ICoproduct2<T1, T2>
    {
        /// <summary>
        /// Creates a new 2-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct2(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 2-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct2(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 2-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct2(ICoproduct2<T1, T2> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 2-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct2(int discriminator, object value)
            : base(2, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 3-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct3
    {
        /// <summary>
        /// Creates a new 3-dimensional coproduct with the third value.
        /// </summary>
        public static ICoproduct3<T1, T2, T3> CreateFirst<T1, T2, T3>(T1 value)
        {
            return new Coproduct3<T1, T2, T3>(value);
        }

        /// <summary>
        /// Creates a new 3-dimensional coproduct with the third value.
        /// </summary>
        public static ICoproduct3<T1, T2, T3> CreateSecond<T1, T2, T3>(T2 value)
        {
            return new Coproduct3<T1, T2, T3>(value);
        }

        /// <summary>
        /// Creates a new 3-dimensional coproduct with the third value.
        /// </summary>
        public static ICoproduct3<T1, T2, T3> CreateThird<T1, T2, T3>(T3 value)
        {
            return new Coproduct3<T1, T2, T3>(value);
        }

    }

    /// <summary>
    /// A 3-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct3<T1, T2, T3> : CoproductBase, ICoproduct3<T1, T2, T3>
    {
        /// <summary>
        /// Creates a new 3-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct3(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 3-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct3(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 3-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct3(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 3-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct3(ICoproduct3<T1, T2, T3> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 3-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct3(int discriminator, object value)
            : base(3, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 4-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct4
    {
        /// <summary>
        /// Creates a new 4-dimensional coproduct with the fourth value.
        /// </summary>
        public static ICoproduct4<T1, T2, T3, T4> CreateFirst<T1, T2, T3, T4>(T1 value)
        {
            return new Coproduct4<T1, T2, T3, T4>(value);
        }

        /// <summary>
        /// Creates a new 4-dimensional coproduct with the fourth value.
        /// </summary>
        public static ICoproduct4<T1, T2, T3, T4> CreateSecond<T1, T2, T3, T4>(T2 value)
        {
            return new Coproduct4<T1, T2, T3, T4>(value);
        }

        /// <summary>
        /// Creates a new 4-dimensional coproduct with the fourth value.
        /// </summary>
        public static ICoproduct4<T1, T2, T3, T4> CreateThird<T1, T2, T3, T4>(T3 value)
        {
            return new Coproduct4<T1, T2, T3, T4>(value);
        }

        /// <summary>
        /// Creates a new 4-dimensional coproduct with the fourth value.
        /// </summary>
        public static ICoproduct4<T1, T2, T3, T4> CreateFourth<T1, T2, T3, T4>(T4 value)
        {
            return new Coproduct4<T1, T2, T3, T4>(value);
        }

    }

    /// <summary>
    /// A 4-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct4<T1, T2, T3, T4> : CoproductBase, ICoproduct4<T1, T2, T3, T4>
    {
        /// <summary>
        /// Creates a new 4-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct4(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 4-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct4(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 4-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct4(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 4-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct4(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 4-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct4(ICoproduct4<T1, T2, T3, T4> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 4-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct4(int discriminator, object value)
            : base(4, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 5-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct5
    {
        /// <summary>
        /// Creates a new 5-dimensional coproduct with the fifth value.
        /// </summary>
        public static ICoproduct5<T1, T2, T3, T4, T5> CreateFirst<T1, T2, T3, T4, T5>(T1 value)
        {
            return new Coproduct5<T1, T2, T3, T4, T5>(value);
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct with the fifth value.
        /// </summary>
        public static ICoproduct5<T1, T2, T3, T4, T5> CreateSecond<T1, T2, T3, T4, T5>(T2 value)
        {
            return new Coproduct5<T1, T2, T3, T4, T5>(value);
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct with the fifth value.
        /// </summary>
        public static ICoproduct5<T1, T2, T3, T4, T5> CreateThird<T1, T2, T3, T4, T5>(T3 value)
        {
            return new Coproduct5<T1, T2, T3, T4, T5>(value);
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct with the fifth value.
        /// </summary>
        public static ICoproduct5<T1, T2, T3, T4, T5> CreateFourth<T1, T2, T3, T4, T5>(T4 value)
        {
            return new Coproduct5<T1, T2, T3, T4, T5>(value);
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct with the fifth value.
        /// </summary>
        public static ICoproduct5<T1, T2, T3, T4, T5> CreateFifth<T1, T2, T3, T4, T5>(T5 value)
        {
            return new Coproduct5<T1, T2, T3, T4, T5>(value);
        }

    }

    /// <summary>
    /// A 5-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct5<T1, T2, T3, T4, T5> : CoproductBase, ICoproduct5<T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// Creates a new 5-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct5(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct5(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct5(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct5(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct5(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct5(ICoproduct5<T1, T2, T3, T4, T5> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 5-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct5(int discriminator, object value)
            : base(5, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 6-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct6
    {
        /// <summary>
        /// Creates a new 6-dimensional coproduct with the sixth value.
        /// </summary>
        public static ICoproduct6<T1, T2, T3, T4, T5, T6> CreateFirst<T1, T2, T3, T4, T5, T6>(T1 value)
        {
            return new Coproduct6<T1, T2, T3, T4, T5, T6>(value);
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the sixth value.
        /// </summary>
        public static ICoproduct6<T1, T2, T3, T4, T5, T6> CreateSecond<T1, T2, T3, T4, T5, T6>(T2 value)
        {
            return new Coproduct6<T1, T2, T3, T4, T5, T6>(value);
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the sixth value.
        /// </summary>
        public static ICoproduct6<T1, T2, T3, T4, T5, T6> CreateThird<T1, T2, T3, T4, T5, T6>(T3 value)
        {
            return new Coproduct6<T1, T2, T3, T4, T5, T6>(value);
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the sixth value.
        /// </summary>
        public static ICoproduct6<T1, T2, T3, T4, T5, T6> CreateFourth<T1, T2, T3, T4, T5, T6>(T4 value)
        {
            return new Coproduct6<T1, T2, T3, T4, T5, T6>(value);
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the sixth value.
        /// </summary>
        public static ICoproduct6<T1, T2, T3, T4, T5, T6> CreateFifth<T1, T2, T3, T4, T5, T6>(T5 value)
        {
            return new Coproduct6<T1, T2, T3, T4, T5, T6>(value);
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the sixth value.
        /// </summary>
        public static ICoproduct6<T1, T2, T3, T4, T5, T6> CreateSixth<T1, T2, T3, T4, T5, T6>(T6 value)
        {
            return new Coproduct6<T1, T2, T3, T4, T5, T6>(value);
        }

    }

    /// <summary>
    /// A 6-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct6<T1, T2, T3, T4, T5, T6> : CoproductBase, ICoproduct6<T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// Creates a new 6-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct6(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct6(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct6(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct6(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct6(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct6(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct6(ICoproduct6<T1, T2, T3, T4, T5, T6> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 6-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct6(int discriminator, object value)
            : base(6, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 7-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct7
    {
        /// <summary>
        /// Creates a new 7-dimensional coproduct with the seventh value.
        /// </summary>
        public static ICoproduct7<T1, T2, T3, T4, T5, T6, T7> CreateFirst<T1, T2, T3, T4, T5, T6, T7>(T1 value)
        {
            return new Coproduct7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the seventh value.
        /// </summary>
        public static ICoproduct7<T1, T2, T3, T4, T5, T6, T7> CreateSecond<T1, T2, T3, T4, T5, T6, T7>(T2 value)
        {
            return new Coproduct7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the seventh value.
        /// </summary>
        public static ICoproduct7<T1, T2, T3, T4, T5, T6, T7> CreateThird<T1, T2, T3, T4, T5, T6, T7>(T3 value)
        {
            return new Coproduct7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the seventh value.
        /// </summary>
        public static ICoproduct7<T1, T2, T3, T4, T5, T6, T7> CreateFourth<T1, T2, T3, T4, T5, T6, T7>(T4 value)
        {
            return new Coproduct7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the seventh value.
        /// </summary>
        public static ICoproduct7<T1, T2, T3, T4, T5, T6, T7> CreateFifth<T1, T2, T3, T4, T5, T6, T7>(T5 value)
        {
            return new Coproduct7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the seventh value.
        /// </summary>
        public static ICoproduct7<T1, T2, T3, T4, T5, T6, T7> CreateSixth<T1, T2, T3, T4, T5, T6, T7>(T6 value)
        {
            return new Coproduct7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the seventh value.
        /// </summary>
        public static ICoproduct7<T1, T2, T3, T4, T5, T6, T7> CreateSeventh<T1, T2, T3, T4, T5, T6, T7>(T7 value)
        {
            return new Coproduct7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

    }

    /// <summary>
    /// A 7-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct7<T1, T2, T3, T4, T5, T6, T7> : CoproductBase, ICoproduct7<T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        /// Creates a new 7-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct7(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct7(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct7(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct7(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct7(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct7(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct7(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct7(ICoproduct7<T1, T2, T3, T4, T5, T6, T7> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 7-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct7(int discriminator, object value)
            : base(7, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 8-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct8
    {
        /// <summary>
        /// Creates a new 8-dimensional coproduct with the eighth value.
        /// </summary>
        public static ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value)
        {
            return new Coproduct8<T1, T2, T3, T4, T5, T6, T7, T8>(value);
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the eighth value.
        /// </summary>
        public static ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8>(T2 value)
        {
            return new Coproduct8<T1, T2, T3, T4, T5, T6, T7, T8>(value);
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the eighth value.
        /// </summary>
        public static ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8>(T3 value)
        {
            return new Coproduct8<T1, T2, T3, T4, T5, T6, T7, T8>(value);
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the eighth value.
        /// </summary>
        public static ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8>(T4 value)
        {
            return new Coproduct8<T1, T2, T3, T4, T5, T6, T7, T8>(value);
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the eighth value.
        /// </summary>
        public static ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8>(T5 value)
        {
            return new Coproduct8<T1, T2, T3, T4, T5, T6, T7, T8>(value);
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the eighth value.
        /// </summary>
        public static ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8>(T6 value)
        {
            return new Coproduct8<T1, T2, T3, T4, T5, T6, T7, T8>(value);
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the eighth value.
        /// </summary>
        public static ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8>(T7 value)
        {
            return new Coproduct8<T1, T2, T3, T4, T5, T6, T7, T8>(value);
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the eighth value.
        /// </summary>
        public static ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8>(T8 value)
        {
            return new Coproduct8<T1, T2, T3, T4, T5, T6, T7, T8>(value);
        }

    }

    /// <summary>
    /// A 8-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct8<T1, T2, T3, T4, T5, T6, T7, T8> : CoproductBase, ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        /// <summary>
        /// Creates a new 8-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct8(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct8(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct8(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct8(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct8(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct8(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct8(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct8(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct8(ICoproduct8<T1, T2, T3, T4, T5, T6, T7, T8> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 8-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct8(int discriminator, object value)
            : base(8, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 9-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct9
    {
        /// <summary>
        /// Creates a new 9-dimensional coproduct with the ninth value.
        /// </summary>
        public static ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 value)
        {
            return new Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the ninth value.
        /// </summary>
        public static ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T2 value)
        {
            return new Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the ninth value.
        /// </summary>
        public static ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T3 value)
        {
            return new Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the ninth value.
        /// </summary>
        public static ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T4 value)
        {
            return new Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the ninth value.
        /// </summary>
        public static ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T5 value)
        {
            return new Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the ninth value.
        /// </summary>
        public static ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T6 value)
        {
            return new Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the ninth value.
        /// </summary>
        public static ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T7 value)
        {
            return new Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the ninth value.
        /// </summary>
        public static ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T8 value)
        {
            return new Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the ninth value.
        /// </summary>
        public static ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T9 value)
        {
            return new Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>(value);
        }

    }

    /// <summary>
    /// A 9-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> : CoproductBase, ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        /// <summary>
        /// Creates a new 9-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct9(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct9(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct9(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct9(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct9(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct9(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct9(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct9(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct9(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct9(ICoproduct9<T1, T2, T3, T4, T5, T6, T7, T8, T9> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 9-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct9(int discriminator, object value)
            : base(9, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 10-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct10
    {
        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T2 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T3 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T4 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T5 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T6 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T7 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T8 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T9 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the tenth value.
        /// </summary>
        public static ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T10 value)
        {
            return new Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(value);
        }

    }

    /// <summary>
    /// A 10-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : CoproductBase, ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct10(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct10(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct10(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct10(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct10(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct10(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct10(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct10(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct10(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct10(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct10(ICoproduct10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 10-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct10(int discriminator, object value)
            : base(10, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 11-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct11
    {
        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T2 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T3 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T4 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T5 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T6 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T7 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T8 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T9 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T10 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the eleventh value.
        /// </summary>
        public static ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateEleventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T11 value)
        {
            return new Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(value);
        }

    }

    /// <summary>
    /// A 11-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : CoproductBase, ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct11(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct11(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct11(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct11(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct11(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct11(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct11(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct11(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct11(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct11(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct with the specified value on the eleventh position.
        /// </summary>
        public Coproduct11(T11 eleventhValue)
            : this(11, eleventhValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct11(ICoproduct11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 11-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct11(int discriminator, object value)
            : base(11, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }
        public bool IsEleventh
        {
            get { return CoproductDiscriminator == 11; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }
        public IOption<T11> Eleventh
        {
            get { return IsEleventh ? Option.Valued(GetCoproductValue<T11>()) : Option.Empty<T11>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth,
            Func<T11, R> ifEleventh)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                case 11: return ifEleventh(GetCoproductValue<T11>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null,
            Action<T11> ifEleventh = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
                case 11: ifEleventh?.Invoke(GetCoproductValue<T11>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 12-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct12
    {
        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T2 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T3 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T4 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T5 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T6 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T7 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T8 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T9 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T10 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateEleventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T11 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the twelfth value.
        /// </summary>
        public static ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateTwelfth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T12 value)
        {
            return new Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(value);
        }

    }

    /// <summary>
    /// A 12-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : CoproductBase, ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct12(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct12(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct12(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct12(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct12(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct12(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct12(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct12(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct12(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct12(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the eleventh position.
        /// </summary>
        public Coproduct12(T11 eleventhValue)
            : this(11, eleventhValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct with the specified value on the twelfth position.
        /// </summary>
        public Coproduct12(T12 twelfthValue)
            : this(12, twelfthValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct12(ICoproduct12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 12-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct12(int discriminator, object value)
            : base(12, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }
        public bool IsEleventh
        {
            get { return CoproductDiscriminator == 11; }
        }
        public bool IsTwelfth
        {
            get { return CoproductDiscriminator == 12; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }
        public IOption<T11> Eleventh
        {
            get { return IsEleventh ? Option.Valued(GetCoproductValue<T11>()) : Option.Empty<T11>(); }
        }
        public IOption<T12> Twelfth
        {
            get { return IsTwelfth ? Option.Valued(GetCoproductValue<T12>()) : Option.Empty<T12>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth,
            Func<T11, R> ifEleventh,
            Func<T12, R> ifTwelfth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                case 11: return ifEleventh(GetCoproductValue<T11>());
                case 12: return ifTwelfth(GetCoproductValue<T12>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null,
            Action<T11> ifEleventh = null,
            Action<T12> ifTwelfth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
                case 11: ifEleventh?.Invoke(GetCoproductValue<T11>()); break;
                case 12: ifTwelfth?.Invoke(GetCoproductValue<T12>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 13-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct13
    {
        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T2 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T3 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T4 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T5 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T6 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T7 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T8 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T9 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T10 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateEleventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T11 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateTwelfth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T12 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the thirteenth value.
        /// </summary>
        public static ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateThirteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T13 value)
        {
            return new Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(value);
        }

    }

    /// <summary>
    /// A 13-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : CoproductBase, ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct13(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct13(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct13(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct13(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct13(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct13(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct13(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct13(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct13(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct13(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the eleventh position.
        /// </summary>
        public Coproduct13(T11 eleventhValue)
            : this(11, eleventhValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the twelfth position.
        /// </summary>
        public Coproduct13(T12 twelfthValue)
            : this(12, twelfthValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct with the specified value on the thirteenth position.
        /// </summary>
        public Coproduct13(T13 thirteenthValue)
            : this(13, thirteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct13(ICoproduct13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 13-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct13(int discriminator, object value)
            : base(13, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }
        public bool IsEleventh
        {
            get { return CoproductDiscriminator == 11; }
        }
        public bool IsTwelfth
        {
            get { return CoproductDiscriminator == 12; }
        }
        public bool IsThirteenth
        {
            get { return CoproductDiscriminator == 13; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }
        public IOption<T11> Eleventh
        {
            get { return IsEleventh ? Option.Valued(GetCoproductValue<T11>()) : Option.Empty<T11>(); }
        }
        public IOption<T12> Twelfth
        {
            get { return IsTwelfth ? Option.Valued(GetCoproductValue<T12>()) : Option.Empty<T12>(); }
        }
        public IOption<T13> Thirteenth
        {
            get { return IsThirteenth ? Option.Valued(GetCoproductValue<T13>()) : Option.Empty<T13>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth,
            Func<T11, R> ifEleventh,
            Func<T12, R> ifTwelfth,
            Func<T13, R> ifThirteenth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                case 11: return ifEleventh(GetCoproductValue<T11>());
                case 12: return ifTwelfth(GetCoproductValue<T12>());
                case 13: return ifThirteenth(GetCoproductValue<T13>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null,
            Action<T11> ifEleventh = null,
            Action<T12> ifTwelfth = null,
            Action<T13> ifThirteenth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
                case 11: ifEleventh?.Invoke(GetCoproductValue<T11>()); break;
                case 12: ifTwelfth?.Invoke(GetCoproductValue<T12>()); break;
                case 13: ifThirteenth?.Invoke(GetCoproductValue<T13>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 14-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct14
    {
        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T2 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T3 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T4 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T5 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T6 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T7 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T8 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T9 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T10 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateEleventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T11 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateTwelfth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T12 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateThirteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T13 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the fourteenth value.
        /// </summary>
        public static ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateFourteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T14 value)
        {
            return new Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(value);
        }

    }

    /// <summary>
    /// A 14-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : CoproductBase, ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct14(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct14(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct14(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct14(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct14(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct14(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct14(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct14(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct14(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct14(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the eleventh position.
        /// </summary>
        public Coproduct14(T11 eleventhValue)
            : this(11, eleventhValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the twelfth position.
        /// </summary>
        public Coproduct14(T12 twelfthValue)
            : this(12, twelfthValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the thirteenth position.
        /// </summary>
        public Coproduct14(T13 thirteenthValue)
            : this(13, thirteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct with the specified value on the fourteenth position.
        /// </summary>
        public Coproduct14(T14 fourteenthValue)
            : this(14, fourteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct14(ICoproduct14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 14-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct14(int discriminator, object value)
            : base(14, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }
        public bool IsEleventh
        {
            get { return CoproductDiscriminator == 11; }
        }
        public bool IsTwelfth
        {
            get { return CoproductDiscriminator == 12; }
        }
        public bool IsThirteenth
        {
            get { return CoproductDiscriminator == 13; }
        }
        public bool IsFourteenth
        {
            get { return CoproductDiscriminator == 14; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }
        public IOption<T11> Eleventh
        {
            get { return IsEleventh ? Option.Valued(GetCoproductValue<T11>()) : Option.Empty<T11>(); }
        }
        public IOption<T12> Twelfth
        {
            get { return IsTwelfth ? Option.Valued(GetCoproductValue<T12>()) : Option.Empty<T12>(); }
        }
        public IOption<T13> Thirteenth
        {
            get { return IsThirteenth ? Option.Valued(GetCoproductValue<T13>()) : Option.Empty<T13>(); }
        }
        public IOption<T14> Fourteenth
        {
            get { return IsFourteenth ? Option.Valued(GetCoproductValue<T14>()) : Option.Empty<T14>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth,
            Func<T11, R> ifEleventh,
            Func<T12, R> ifTwelfth,
            Func<T13, R> ifThirteenth,
            Func<T14, R> ifFourteenth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                case 11: return ifEleventh(GetCoproductValue<T11>());
                case 12: return ifTwelfth(GetCoproductValue<T12>());
                case 13: return ifThirteenth(GetCoproductValue<T13>());
                case 14: return ifFourteenth(GetCoproductValue<T14>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null,
            Action<T11> ifEleventh = null,
            Action<T12> ifTwelfth = null,
            Action<T13> ifThirteenth = null,
            Action<T14> ifFourteenth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
                case 11: ifEleventh?.Invoke(GetCoproductValue<T11>()); break;
                case 12: ifTwelfth?.Invoke(GetCoproductValue<T12>()); break;
                case 13: ifThirteenth?.Invoke(GetCoproductValue<T13>()); break;
                case 14: ifFourteenth?.Invoke(GetCoproductValue<T14>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 15-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct15
    {
        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T2 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T3 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T4 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T5 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T6 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T7 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T8 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T9 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T10 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateEleventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T11 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateTwelfth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T12 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateThirteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T13 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateFourteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T14 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the fifteenth value.
        /// </summary>
        public static ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateFifteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T15 value)
        {
            return new Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(value);
        }

    }

    /// <summary>
    /// A 15-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : CoproductBase, ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct15(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct15(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct15(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct15(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct15(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct15(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct15(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct15(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct15(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct15(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the eleventh position.
        /// </summary>
        public Coproduct15(T11 eleventhValue)
            : this(11, eleventhValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the twelfth position.
        /// </summary>
        public Coproduct15(T12 twelfthValue)
            : this(12, twelfthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the thirteenth position.
        /// </summary>
        public Coproduct15(T13 thirteenthValue)
            : this(13, thirteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the fourteenth position.
        /// </summary>
        public Coproduct15(T14 fourteenthValue)
            : this(14, fourteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct with the specified value on the fifteenth position.
        /// </summary>
        public Coproduct15(T15 fifteenthValue)
            : this(15, fifteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct15(ICoproduct15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 15-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct15(int discriminator, object value)
            : base(15, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }
        public bool IsEleventh
        {
            get { return CoproductDiscriminator == 11; }
        }
        public bool IsTwelfth
        {
            get { return CoproductDiscriminator == 12; }
        }
        public bool IsThirteenth
        {
            get { return CoproductDiscriminator == 13; }
        }
        public bool IsFourteenth
        {
            get { return CoproductDiscriminator == 14; }
        }
        public bool IsFifteenth
        {
            get { return CoproductDiscriminator == 15; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }
        public IOption<T11> Eleventh
        {
            get { return IsEleventh ? Option.Valued(GetCoproductValue<T11>()) : Option.Empty<T11>(); }
        }
        public IOption<T12> Twelfth
        {
            get { return IsTwelfth ? Option.Valued(GetCoproductValue<T12>()) : Option.Empty<T12>(); }
        }
        public IOption<T13> Thirteenth
        {
            get { return IsThirteenth ? Option.Valued(GetCoproductValue<T13>()) : Option.Empty<T13>(); }
        }
        public IOption<T14> Fourteenth
        {
            get { return IsFourteenth ? Option.Valued(GetCoproductValue<T14>()) : Option.Empty<T14>(); }
        }
        public IOption<T15> Fifteenth
        {
            get { return IsFifteenth ? Option.Valued(GetCoproductValue<T15>()) : Option.Empty<T15>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth,
            Func<T11, R> ifEleventh,
            Func<T12, R> ifTwelfth,
            Func<T13, R> ifThirteenth,
            Func<T14, R> ifFourteenth,
            Func<T15, R> ifFifteenth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                case 11: return ifEleventh(GetCoproductValue<T11>());
                case 12: return ifTwelfth(GetCoproductValue<T12>());
                case 13: return ifThirteenth(GetCoproductValue<T13>());
                case 14: return ifFourteenth(GetCoproductValue<T14>());
                case 15: return ifFifteenth(GetCoproductValue<T15>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null,
            Action<T11> ifEleventh = null,
            Action<T12> ifTwelfth = null,
            Action<T13> ifThirteenth = null,
            Action<T14> ifFourteenth = null,
            Action<T15> ifFifteenth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
                case 11: ifEleventh?.Invoke(GetCoproductValue<T11>()); break;
                case 12: ifTwelfth?.Invoke(GetCoproductValue<T12>()); break;
                case 13: ifThirteenth?.Invoke(GetCoproductValue<T13>()); break;
                case 14: ifFourteenth?.Invoke(GetCoproductValue<T14>()); break;
                case 15: ifFifteenth?.Invoke(GetCoproductValue<T15>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 16-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct16
    {
        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T2 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T3 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T4 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T5 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T6 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T7 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T8 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T9 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T10 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateEleventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T11 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateTwelfth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T12 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateThirteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T13 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateFourteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T14 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateFifteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T15 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the sixteenth value.
        /// </summary>
        public static ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateSixteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T16 value)
        {
            return new Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(value);
        }

    }

    /// <summary>
    /// A 16-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : CoproductBase, ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {
        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct16(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct16(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct16(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct16(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct16(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct16(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct16(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct16(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct16(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct16(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the eleventh position.
        /// </summary>
        public Coproduct16(T11 eleventhValue)
            : this(11, eleventhValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the twelfth position.
        /// </summary>
        public Coproduct16(T12 twelfthValue)
            : this(12, twelfthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the thirteenth position.
        /// </summary>
        public Coproduct16(T13 thirteenthValue)
            : this(13, thirteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the fourteenth position.
        /// </summary>
        public Coproduct16(T14 fourteenthValue)
            : this(14, fourteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the fifteenth position.
        /// </summary>
        public Coproduct16(T15 fifteenthValue)
            : this(15, fifteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct with the specified value on the sixteenth position.
        /// </summary>
        public Coproduct16(T16 sixteenthValue)
            : this(16, sixteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct16(ICoproduct16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 16-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct16(int discriminator, object value)
            : base(16, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }
        public bool IsEleventh
        {
            get { return CoproductDiscriminator == 11; }
        }
        public bool IsTwelfth
        {
            get { return CoproductDiscriminator == 12; }
        }
        public bool IsThirteenth
        {
            get { return CoproductDiscriminator == 13; }
        }
        public bool IsFourteenth
        {
            get { return CoproductDiscriminator == 14; }
        }
        public bool IsFifteenth
        {
            get { return CoproductDiscriminator == 15; }
        }
        public bool IsSixteenth
        {
            get { return CoproductDiscriminator == 16; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }
        public IOption<T11> Eleventh
        {
            get { return IsEleventh ? Option.Valued(GetCoproductValue<T11>()) : Option.Empty<T11>(); }
        }
        public IOption<T12> Twelfth
        {
            get { return IsTwelfth ? Option.Valued(GetCoproductValue<T12>()) : Option.Empty<T12>(); }
        }
        public IOption<T13> Thirteenth
        {
            get { return IsThirteenth ? Option.Valued(GetCoproductValue<T13>()) : Option.Empty<T13>(); }
        }
        public IOption<T14> Fourteenth
        {
            get { return IsFourteenth ? Option.Valued(GetCoproductValue<T14>()) : Option.Empty<T14>(); }
        }
        public IOption<T15> Fifteenth
        {
            get { return IsFifteenth ? Option.Valued(GetCoproductValue<T15>()) : Option.Empty<T15>(); }
        }
        public IOption<T16> Sixteenth
        {
            get { return IsSixteenth ? Option.Valued(GetCoproductValue<T16>()) : Option.Empty<T16>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth,
            Func<T11, R> ifEleventh,
            Func<T12, R> ifTwelfth,
            Func<T13, R> ifThirteenth,
            Func<T14, R> ifFourteenth,
            Func<T15, R> ifFifteenth,
            Func<T16, R> ifSixteenth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                case 11: return ifEleventh(GetCoproductValue<T11>());
                case 12: return ifTwelfth(GetCoproductValue<T12>());
                case 13: return ifThirteenth(GetCoproductValue<T13>());
                case 14: return ifFourteenth(GetCoproductValue<T14>());
                case 15: return ifFifteenth(GetCoproductValue<T15>());
                case 16: return ifSixteenth(GetCoproductValue<T16>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null,
            Action<T11> ifEleventh = null,
            Action<T12> ifTwelfth = null,
            Action<T13> ifThirteenth = null,
            Action<T14> ifFourteenth = null,
            Action<T15> ifFifteenth = null,
            Action<T16> ifSixteenth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
                case 11: ifEleventh?.Invoke(GetCoproductValue<T11>()); break;
                case 12: ifTwelfth?.Invoke(GetCoproductValue<T12>()); break;
                case 13: ifThirteenth?.Invoke(GetCoproductValue<T13>()); break;
                case 14: ifFourteenth?.Invoke(GetCoproductValue<T14>()); break;
                case 15: ifFifteenth?.Invoke(GetCoproductValue<T15>()); break;
                case 16: ifSixteenth?.Invoke(GetCoproductValue<T16>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 17-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct17
    {
        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T2 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T3 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T4 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T5 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T6 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T7 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T8 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T9 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T10 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateEleventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T11 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateTwelfth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T12 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateThirteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T13 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateFourteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T14 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateFifteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T15 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateSixteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T16 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the seventeenth value.
        /// </summary>
        public static ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateSeventeenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T17 value)
        {
            return new Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(value);
        }

    }

    /// <summary>
    /// A 17-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : CoproductBase, ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
    {
        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct17(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct17(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct17(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct17(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct17(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct17(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct17(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct17(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct17(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct17(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the eleventh position.
        /// </summary>
        public Coproduct17(T11 eleventhValue)
            : this(11, eleventhValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the twelfth position.
        /// </summary>
        public Coproduct17(T12 twelfthValue)
            : this(12, twelfthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the thirteenth position.
        /// </summary>
        public Coproduct17(T13 thirteenthValue)
            : this(13, thirteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the fourteenth position.
        /// </summary>
        public Coproduct17(T14 fourteenthValue)
            : this(14, fourteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the fifteenth position.
        /// </summary>
        public Coproduct17(T15 fifteenthValue)
            : this(15, fifteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the sixteenth position.
        /// </summary>
        public Coproduct17(T16 sixteenthValue)
            : this(16, sixteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct with the specified value on the seventeenth position.
        /// </summary>
        public Coproduct17(T17 seventeenthValue)
            : this(17, seventeenthValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct17(ICoproduct17<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 17-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct17(int discriminator, object value)
            : base(17, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }
        public bool IsEleventh
        {
            get { return CoproductDiscriminator == 11; }
        }
        public bool IsTwelfth
        {
            get { return CoproductDiscriminator == 12; }
        }
        public bool IsThirteenth
        {
            get { return CoproductDiscriminator == 13; }
        }
        public bool IsFourteenth
        {
            get { return CoproductDiscriminator == 14; }
        }
        public bool IsFifteenth
        {
            get { return CoproductDiscriminator == 15; }
        }
        public bool IsSixteenth
        {
            get { return CoproductDiscriminator == 16; }
        }
        public bool IsSeventeenth
        {
            get { return CoproductDiscriminator == 17; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }
        public IOption<T11> Eleventh
        {
            get { return IsEleventh ? Option.Valued(GetCoproductValue<T11>()) : Option.Empty<T11>(); }
        }
        public IOption<T12> Twelfth
        {
            get { return IsTwelfth ? Option.Valued(GetCoproductValue<T12>()) : Option.Empty<T12>(); }
        }
        public IOption<T13> Thirteenth
        {
            get { return IsThirteenth ? Option.Valued(GetCoproductValue<T13>()) : Option.Empty<T13>(); }
        }
        public IOption<T14> Fourteenth
        {
            get { return IsFourteenth ? Option.Valued(GetCoproductValue<T14>()) : Option.Empty<T14>(); }
        }
        public IOption<T15> Fifteenth
        {
            get { return IsFifteenth ? Option.Valued(GetCoproductValue<T15>()) : Option.Empty<T15>(); }
        }
        public IOption<T16> Sixteenth
        {
            get { return IsSixteenth ? Option.Valued(GetCoproductValue<T16>()) : Option.Empty<T16>(); }
        }
        public IOption<T17> Seventeenth
        {
            get { return IsSeventeenth ? Option.Valued(GetCoproductValue<T17>()) : Option.Empty<T17>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth,
            Func<T11, R> ifEleventh,
            Func<T12, R> ifTwelfth,
            Func<T13, R> ifThirteenth,
            Func<T14, R> ifFourteenth,
            Func<T15, R> ifFifteenth,
            Func<T16, R> ifSixteenth,
            Func<T17, R> ifSeventeenth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                case 11: return ifEleventh(GetCoproductValue<T11>());
                case 12: return ifTwelfth(GetCoproductValue<T12>());
                case 13: return ifThirteenth(GetCoproductValue<T13>());
                case 14: return ifFourteenth(GetCoproductValue<T14>());
                case 15: return ifFifteenth(GetCoproductValue<T15>());
                case 16: return ifSixteenth(GetCoproductValue<T16>());
                case 17: return ifSeventeenth(GetCoproductValue<T17>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null,
            Action<T11> ifEleventh = null,
            Action<T12> ifTwelfth = null,
            Action<T13> ifThirteenth = null,
            Action<T14> ifFourteenth = null,
            Action<T15> ifFifteenth = null,
            Action<T16> ifSixteenth = null,
            Action<T17> ifSeventeenth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
                case 11: ifEleventh?.Invoke(GetCoproductValue<T11>()); break;
                case 12: ifTwelfth?.Invoke(GetCoproductValue<T12>()); break;
                case 13: ifThirteenth?.Invoke(GetCoproductValue<T13>()); break;
                case 14: ifFourteenth?.Invoke(GetCoproductValue<T14>()); break;
                case 15: ifFifteenth?.Invoke(GetCoproductValue<T15>()); break;
                case 16: ifSixteenth?.Invoke(GetCoproductValue<T16>()); break;
                case 17: ifSeventeenth?.Invoke(GetCoproductValue<T17>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 18-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct18
    {
        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T1 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T2 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T3 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T4 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T5 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T6 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T7 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T8 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T9 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T10 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateEleventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T11 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateTwelfth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T12 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateThirteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T13 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateFourteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T14 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateFifteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T15 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateSixteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T16 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateSeventeenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T17 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the eighteenth value.
        /// </summary>
        public static ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateEighteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T18 value)
        {
            return new Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(value);
        }

    }

    /// <summary>
    /// A 18-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : CoproductBase, ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
    {
        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct18(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct18(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct18(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct18(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct18(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct18(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct18(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct18(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct18(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct18(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the eleventh position.
        /// </summary>
        public Coproduct18(T11 eleventhValue)
            : this(11, eleventhValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the twelfth position.
        /// </summary>
        public Coproduct18(T12 twelfthValue)
            : this(12, twelfthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the thirteenth position.
        /// </summary>
        public Coproduct18(T13 thirteenthValue)
            : this(13, thirteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the fourteenth position.
        /// </summary>
        public Coproduct18(T14 fourteenthValue)
            : this(14, fourteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the fifteenth position.
        /// </summary>
        public Coproduct18(T15 fifteenthValue)
            : this(15, fifteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the sixteenth position.
        /// </summary>
        public Coproduct18(T16 sixteenthValue)
            : this(16, sixteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the seventeenth position.
        /// </summary>
        public Coproduct18(T17 seventeenthValue)
            : this(17, seventeenthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct with the specified value on the eighteenth position.
        /// </summary>
        public Coproduct18(T18 eighteenthValue)
            : this(18, eighteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct18(ICoproduct18<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 18-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct18(int discriminator, object value)
            : base(18, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }
        public bool IsEleventh
        {
            get { return CoproductDiscriminator == 11; }
        }
        public bool IsTwelfth
        {
            get { return CoproductDiscriminator == 12; }
        }
        public bool IsThirteenth
        {
            get { return CoproductDiscriminator == 13; }
        }
        public bool IsFourteenth
        {
            get { return CoproductDiscriminator == 14; }
        }
        public bool IsFifteenth
        {
            get { return CoproductDiscriminator == 15; }
        }
        public bool IsSixteenth
        {
            get { return CoproductDiscriminator == 16; }
        }
        public bool IsSeventeenth
        {
            get { return CoproductDiscriminator == 17; }
        }
        public bool IsEighteenth
        {
            get { return CoproductDiscriminator == 18; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }
        public IOption<T11> Eleventh
        {
            get { return IsEleventh ? Option.Valued(GetCoproductValue<T11>()) : Option.Empty<T11>(); }
        }
        public IOption<T12> Twelfth
        {
            get { return IsTwelfth ? Option.Valued(GetCoproductValue<T12>()) : Option.Empty<T12>(); }
        }
        public IOption<T13> Thirteenth
        {
            get { return IsThirteenth ? Option.Valued(GetCoproductValue<T13>()) : Option.Empty<T13>(); }
        }
        public IOption<T14> Fourteenth
        {
            get { return IsFourteenth ? Option.Valued(GetCoproductValue<T14>()) : Option.Empty<T14>(); }
        }
        public IOption<T15> Fifteenth
        {
            get { return IsFifteenth ? Option.Valued(GetCoproductValue<T15>()) : Option.Empty<T15>(); }
        }
        public IOption<T16> Sixteenth
        {
            get { return IsSixteenth ? Option.Valued(GetCoproductValue<T16>()) : Option.Empty<T16>(); }
        }
        public IOption<T17> Seventeenth
        {
            get { return IsSeventeenth ? Option.Valued(GetCoproductValue<T17>()) : Option.Empty<T17>(); }
        }
        public IOption<T18> Eighteenth
        {
            get { return IsEighteenth ? Option.Valued(GetCoproductValue<T18>()) : Option.Empty<T18>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth,
            Func<T11, R> ifEleventh,
            Func<T12, R> ifTwelfth,
            Func<T13, R> ifThirteenth,
            Func<T14, R> ifFourteenth,
            Func<T15, R> ifFifteenth,
            Func<T16, R> ifSixteenth,
            Func<T17, R> ifSeventeenth,
            Func<T18, R> ifEighteenth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                case 11: return ifEleventh(GetCoproductValue<T11>());
                case 12: return ifTwelfth(GetCoproductValue<T12>());
                case 13: return ifThirteenth(GetCoproductValue<T13>());
                case 14: return ifFourteenth(GetCoproductValue<T14>());
                case 15: return ifFifteenth(GetCoproductValue<T15>());
                case 16: return ifSixteenth(GetCoproductValue<T16>());
                case 17: return ifSeventeenth(GetCoproductValue<T17>());
                case 18: return ifEighteenth(GetCoproductValue<T18>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null,
            Action<T11> ifEleventh = null,
            Action<T12> ifTwelfth = null,
            Action<T13> ifThirteenth = null,
            Action<T14> ifFourteenth = null,
            Action<T15> ifFifteenth = null,
            Action<T16> ifSixteenth = null,
            Action<T17> ifSeventeenth = null,
            Action<T18> ifEighteenth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
                case 11: ifEleventh?.Invoke(GetCoproductValue<T11>()); break;
                case 12: ifTwelfth?.Invoke(GetCoproductValue<T12>()); break;
                case 13: ifThirteenth?.Invoke(GetCoproductValue<T13>()); break;
                case 14: ifFourteenth?.Invoke(GetCoproductValue<T14>()); break;
                case 15: ifFifteenth?.Invoke(GetCoproductValue<T15>()); break;
                case 16: ifSixteenth?.Invoke(GetCoproductValue<T16>()); break;
                case 17: ifSeventeenth?.Invoke(GetCoproductValue<T17>()); break;
                case 18: ifEighteenth?.Invoke(GetCoproductValue<T18>()); break;
            }
        }
    }

    /// <summary>
    /// Factory for 19-dimensional immutable coproducts.
    /// </summary>
    public static class Coproduct19
    {
        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateFirst<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T1 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateSecond<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T2 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateThird<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T3 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateFourth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T4 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateFifth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T5 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateSixth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T6 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateSeventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T7 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateEighth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T8 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateNinth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T9 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateTenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T10 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateEleventh<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T11 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateTwelfth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T12 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateThirteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T13 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateFourteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T14 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateFifteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T15 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateSixteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T16 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateSeventeenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T17 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateEighteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T18 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the nineteenth value.
        /// </summary>
        public static ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateNineteenth<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T19 value)
        {
            return new Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(value);
        }

    }

    /// <summary>
    /// A 19-dimensional immutable coproduct.
    /// </summary> 
    public class Coproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : CoproductBase, ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
    {
        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the first position.
        /// </summary>
        public Coproduct19(T1 firstValue)
            : this(1, firstValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the second position.
        /// </summary>
        public Coproduct19(T2 secondValue)
            : this(2, secondValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the third position.
        /// </summary>
        public Coproduct19(T3 thirdValue)
            : this(3, thirdValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the fourth position.
        /// </summary>
        public Coproduct19(T4 fourthValue)
            : this(4, fourthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the fifth position.
        /// </summary>
        public Coproduct19(T5 fifthValue)
            : this(5, fifthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the sixth position.
        /// </summary>
        public Coproduct19(T6 sixthValue)
            : this(6, sixthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the seventh position.
        /// </summary>
        public Coproduct19(T7 seventhValue)
            : this(7, seventhValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the eighth position.
        /// </summary>
        public Coproduct19(T8 eighthValue)
            : this(8, eighthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the ninth position.
        /// </summary>
        public Coproduct19(T9 ninthValue)
            : this(9, ninthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the tenth position.
        /// </summary>
        public Coproduct19(T10 tenthValue)
            : this(10, tenthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the eleventh position.
        /// </summary>
        public Coproduct19(T11 eleventhValue)
            : this(11, eleventhValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the twelfth position.
        /// </summary>
        public Coproduct19(T12 twelfthValue)
            : this(12, twelfthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the thirteenth position.
        /// </summary>
        public Coproduct19(T13 thirteenthValue)
            : this(13, thirteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the fourteenth position.
        /// </summary>
        public Coproduct19(T14 fourteenthValue)
            : this(14, fourteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the fifteenth position.
        /// </summary>
        public Coproduct19(T15 fifteenthValue)
            : this(15, fifteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the sixteenth position.
        /// </summary>
        public Coproduct19(T16 sixteenthValue)
            : this(16, sixteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the seventeenth position.
        /// </summary>
        public Coproduct19(T17 seventeenthValue)
            : this(17, seventeenthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the eighteenth position.
        /// </summary>
        public Coproduct19(T18 eighteenthValue)
            : this(18, eighteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct with the specified value on the nineteenth position.
        /// </summary>
        public Coproduct19(T19 nineteenthValue)
            : this(19, nineteenthValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct based on the specified source.
        /// </summary>
        public Coproduct19(ICoproduct19<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> source)
            : this(source.CoproductDiscriminator, source.CoproductValue)
        {
        }

        /// <summary>
        /// Creates a new 19-dimensional coproduct.
        /// </summary>
        /// <param name="discriminator">Discriminator of the value from interval [1, arity].</param>
        /// <param name="value">Value of the coproduct on the position defined by the discriminator.</param>
        protected Coproduct19(int discriminator, object value)
            : base(19, discriminator, value)
        {
        }

        public bool IsFirst
        {
            get { return CoproductDiscriminator == 1; }
        }
        public bool IsSecond
        {
            get { return CoproductDiscriminator == 2; }
        }
        public bool IsThird
        {
            get { return CoproductDiscriminator == 3; }
        }
        public bool IsFourth
        {
            get { return CoproductDiscriminator == 4; }
        }
        public bool IsFifth
        {
            get { return CoproductDiscriminator == 5; }
        }
        public bool IsSixth
        {
            get { return CoproductDiscriminator == 6; }
        }
        public bool IsSeventh
        {
            get { return CoproductDiscriminator == 7; }
        }
        public bool IsEighth
        {
            get { return CoproductDiscriminator == 8; }
        }
        public bool IsNinth
        {
            get { return CoproductDiscriminator == 9; }
        }
        public bool IsTenth
        {
            get { return CoproductDiscriminator == 10; }
        }
        public bool IsEleventh
        {
            get { return CoproductDiscriminator == 11; }
        }
        public bool IsTwelfth
        {
            get { return CoproductDiscriminator == 12; }
        }
        public bool IsThirteenth
        {
            get { return CoproductDiscriminator == 13; }
        }
        public bool IsFourteenth
        {
            get { return CoproductDiscriminator == 14; }
        }
        public bool IsFifteenth
        {
            get { return CoproductDiscriminator == 15; }
        }
        public bool IsSixteenth
        {
            get { return CoproductDiscriminator == 16; }
        }
        public bool IsSeventeenth
        {
            get { return CoproductDiscriminator == 17; }
        }
        public bool IsEighteenth
        {
            get { return CoproductDiscriminator == 18; }
        }
        public bool IsNineteenth
        {
            get { return CoproductDiscriminator == 19; }
        }

        public IOption<T1> First
        {
            get { return IsFirst ? Option.Valued(GetCoproductValue<T1>()) : Option.Empty<T1>(); }
        }
        public IOption<T2> Second
        {
            get { return IsSecond ? Option.Valued(GetCoproductValue<T2>()) : Option.Empty<T2>(); }
        }
        public IOption<T3> Third
        {
            get { return IsThird ? Option.Valued(GetCoproductValue<T3>()) : Option.Empty<T3>(); }
        }
        public IOption<T4> Fourth
        {
            get { return IsFourth ? Option.Valued(GetCoproductValue<T4>()) : Option.Empty<T4>(); }
        }
        public IOption<T5> Fifth
        {
            get { return IsFifth ? Option.Valued(GetCoproductValue<T5>()) : Option.Empty<T5>(); }
        }
        public IOption<T6> Sixth
        {
            get { return IsSixth ? Option.Valued(GetCoproductValue<T6>()) : Option.Empty<T6>(); }
        }
        public IOption<T7> Seventh
        {
            get { return IsSeventh ? Option.Valued(GetCoproductValue<T7>()) : Option.Empty<T7>(); }
        }
        public IOption<T8> Eighth
        {
            get { return IsEighth ? Option.Valued(GetCoproductValue<T8>()) : Option.Empty<T8>(); }
        }
        public IOption<T9> Ninth
        {
            get { return IsNinth ? Option.Valued(GetCoproductValue<T9>()) : Option.Empty<T9>(); }
        }
        public IOption<T10> Tenth
        {
            get { return IsTenth ? Option.Valued(GetCoproductValue<T10>()) : Option.Empty<T10>(); }
        }
        public IOption<T11> Eleventh
        {
            get { return IsEleventh ? Option.Valued(GetCoproductValue<T11>()) : Option.Empty<T11>(); }
        }
        public IOption<T12> Twelfth
        {
            get { return IsTwelfth ? Option.Valued(GetCoproductValue<T12>()) : Option.Empty<T12>(); }
        }
        public IOption<T13> Thirteenth
        {
            get { return IsThirteenth ? Option.Valued(GetCoproductValue<T13>()) : Option.Empty<T13>(); }
        }
        public IOption<T14> Fourteenth
        {
            get { return IsFourteenth ? Option.Valued(GetCoproductValue<T14>()) : Option.Empty<T14>(); }
        }
        public IOption<T15> Fifteenth
        {
            get { return IsFifteenth ? Option.Valued(GetCoproductValue<T15>()) : Option.Empty<T15>(); }
        }
        public IOption<T16> Sixteenth
        {
            get { return IsSixteenth ? Option.Valued(GetCoproductValue<T16>()) : Option.Empty<T16>(); }
        }
        public IOption<T17> Seventeenth
        {
            get { return IsSeventeenth ? Option.Valued(GetCoproductValue<T17>()) : Option.Empty<T17>(); }
        }
        public IOption<T18> Eighteenth
        {
            get { return IsEighteenth ? Option.Valued(GetCoproductValue<T18>()) : Option.Empty<T18>(); }
        }
        public IOption<T19> Nineteenth
        {
            get { return IsNineteenth ? Option.Valued(GetCoproductValue<T19>()) : Option.Empty<T19>(); }
        }

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth,
            Func<T10, R> ifTenth,
            Func<T11, R> ifEleventh,
            Func<T12, R> ifTwelfth,
            Func<T13, R> ifThirteenth,
            Func<T14, R> ifFourteenth,
            Func<T15, R> ifFifteenth,
            Func<T16, R> ifSixteenth,
            Func<T17, R> ifSeventeenth,
            Func<T18, R> ifEighteenth,
            Func<T19, R> ifNineteenth)
        {
            switch (CoproductDiscriminator)
            {
                case 1: return ifFirst(GetCoproductValue<T1>());
                case 2: return ifSecond(GetCoproductValue<T2>());
                case 3: return ifThird(GetCoproductValue<T3>());
                case 4: return ifFourth(GetCoproductValue<T4>());
                case 5: return ifFifth(GetCoproductValue<T5>());
                case 6: return ifSixth(GetCoproductValue<T6>());
                case 7: return ifSeventh(GetCoproductValue<T7>());
                case 8: return ifEighth(GetCoproductValue<T8>());
                case 9: return ifNinth(GetCoproductValue<T9>());
                case 10: return ifTenth(GetCoproductValue<T10>());
                case 11: return ifEleventh(GetCoproductValue<T11>());
                case 12: return ifTwelfth(GetCoproductValue<T12>());
                case 13: return ifThirteenth(GetCoproductValue<T13>());
                case 14: return ifFourteenth(GetCoproductValue<T14>());
                case 15: return ifFifteenth(GetCoproductValue<T15>());
                case 16: return ifSixteenth(GetCoproductValue<T16>());
                case 17: return ifSeventeenth(GetCoproductValue<T17>());
                case 18: return ifEighteenth(GetCoproductValue<T18>());
                case 19: return ifNineteenth(GetCoproductValue<T19>());
                default: return default(R);
            }
        }

        public void Match(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null,
            Action<T10> ifTenth = null,
            Action<T11> ifEleventh = null,
            Action<T12> ifTwelfth = null,
            Action<T13> ifThirteenth = null,
            Action<T14> ifFourteenth = null,
            Action<T15> ifFifteenth = null,
            Action<T16> ifSixteenth = null,
            Action<T17> ifSeventeenth = null,
            Action<T18> ifEighteenth = null,
            Action<T19> ifNineteenth = null)
        {
            switch (CoproductDiscriminator)
            {
                case 1: ifFirst?.Invoke(GetCoproductValue<T1>()); break;
                case 2: ifSecond?.Invoke(GetCoproductValue<T2>()); break;
                case 3: ifThird?.Invoke(GetCoproductValue<T3>()); break;
                case 4: ifFourth?.Invoke(GetCoproductValue<T4>()); break;
                case 5: ifFifth?.Invoke(GetCoproductValue<T5>()); break;
                case 6: ifSixth?.Invoke(GetCoproductValue<T6>()); break;
                case 7: ifSeventh?.Invoke(GetCoproductValue<T7>()); break;
                case 8: ifEighth?.Invoke(GetCoproductValue<T8>()); break;
                case 9: ifNinth?.Invoke(GetCoproductValue<T9>()); break;
                case 10: ifTenth?.Invoke(GetCoproductValue<T10>()); break;
                case 11: ifEleventh?.Invoke(GetCoproductValue<T11>()); break;
                case 12: ifTwelfth?.Invoke(GetCoproductValue<T12>()); break;
                case 13: ifThirteenth?.Invoke(GetCoproductValue<T13>()); break;
                case 14: ifFourteenth?.Invoke(GetCoproductValue<T14>()); break;
                case 15: ifFifteenth?.Invoke(GetCoproductValue<T15>()); break;
                case 16: ifSixteenth?.Invoke(GetCoproductValue<T16>()); break;
                case 17: ifSeventeenth?.Invoke(GetCoproductValue<T17>()); break;
                case 18: ifEighteenth?.Invoke(GetCoproductValue<T18>()); break;
                case 19: ifNineteenth?.Invoke(GetCoproductValue<T19>()); break;
            }
        }
    }

}
