﻿
/* 
 * PROJECT: NyARToolkitCS
 * --------------------------------------------------------------------------------
 *
 * The NyARToolkitCS is C# edition NyARToolKit class library.
 * Copyright (C)2008-2012 Ryo Iizuka
 *
 * This work is based on the ARToolKit developed by
 *   Hirokazu Kato
 *   Mark Billinghurst
 *   HITLab, University of Washington, Seattle
 * http://www.hitl.washington.edu/artoolkit/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as publishe
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * For further information please contact.
 *	http://nyatla.jp/nyatoolkit/
 *	<airmail(at)ebony.plala.or.jp> or <nyatla(at)nyatla.jp>
 * 
 */

using System;

namespace NyAR.Core {

  /**
   * NyARToolkitライブラリが生成するExceptionのクラスです。
   * このクラスは、NyARToolkitライブラリでのみ使用します。
   */
  public class NyARException : Exception {
    private const long serialVersionUID = 1L;

    /**
     * コンストラクタです。
     * 例外オブジェクトを生成します。
     */
    public NyARException() : base() { }

    /**
     * コンストラクタです。
     * 例外オブジェクト継承して、例外を生成します。
     * @param e
     * 継承する例外オブジェクト
     */
    public NyARException(Exception e) : base(e.ToString()) { }

    /**
     * コンストラクタです。
     * メッセージを指定して、例外を生成します。
     * @param m
     */
    public NyARException(string m) : base(m) { }

    /**
     * ライブラリ開発者向けの関数です。
     * 意図的に例外を発生するときに、コードに埋め込みます。
     * @param m
     * 例外メッセージを指定します。
     * @
     */
    public static void Trap(string m) { throw new NyARException("トラップ:" + m); }

    /**
     * ライブラリ開発者向けの関数です。
     * "Not Implement!"メッセージを指定して、例外をスローします。
     * この関数は、NyARToolkitの未実装部分に埋め込みます。
     * @
     */
    public static void NotImplement() { throw new NyARException("Not Implement!"); }

    /**
     * ライブラリ開発者向けの関数です。
     * 関数が使用不能である事を、例外で通知します。
     * @
     */
    public static void Unavailability() { throw new NyARException("unavailability!"); }
  }
}
