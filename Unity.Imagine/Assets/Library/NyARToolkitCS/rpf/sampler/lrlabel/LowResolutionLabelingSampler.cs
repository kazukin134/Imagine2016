
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

using NyAR.Core;

namespace NyAR.Rpf {

  /**
   * 画像データのサンプラです。画像データから、輪郭線抽出のヒントを計算して、出力コンテナに格納します。
   * 入力-LowResolutionLabelingSamplerIn
   * 出力-LowResolutionLabelingSamplerOut
   */
  public class LowResolutionLabelingSampler {

    /**
     * 1/n画像のラべリングをするクラス。
     * @author nyatla
     *
     */
    class Main_Labeling : NyARLabeling_Rle {
      private int _pix;
      public int current_th;
      public LowResolutionLabelingSamplerOut current_output;

      public Main_Labeling(int i_width, int i_height, int i_pix_base) : base(i_width, i_height) { this._pix = i_pix_base; }

      /**
       * @Override
       */
      protected override void OnLabelFound(NyARRleLabelFragmentInfo iRefLabel) {
        //widthとheightの計算
        int w = iRefLabel.clip_r - iRefLabel.clip_l;
        int h = iRefLabel.clip_b - iRefLabel.clip_t;
        //1*1(1bitPixelの5*5)以下の場合は、検出不能
        //未実装部分:2*2(1bitPixelの8*8)以下の場合は、解像度1で再検出
        //未実装部分:3*3,4*4(1bitPixelの12*12,16*16)以下の場合は、解像度2で再検出

        if (w < 10 || h < 10) {
          //今のところは再検出機構なし。
          return;
        }

        LowResolutionLabelingSamplerOut.Item item = current_output.prePush();
        if (item == null) { return; }

        int pix = this._pix;
        item.entry_pos.x = iRefLabel.entry_x;
        item.entry_pos.y = iRefLabel.clip_t;
        item.base_area.x = iRefLabel.clip_l * pix;
        item.base_area.y = iRefLabel.clip_t * pix;
        item.base_area.w = w * pix;
        item.base_area.h = h * pix;
        item.base_area_center.x = item.base_area.x + item.base_area.w / 2;
        item.base_area_center.y = item.base_area.y + item.base_area.h / 2;
        item.base_area_sq_diagonal = (w * w + h * h) * (pix * pix);
        item.lebeling_th = this.current_th;
      }
    }

    private Main_Labeling _main_labeling;

    /**
     * コンストラクタです。samplingするラスターのパラメタを指定して、インスタンスを初期化します。
     * @param i_width
     * サンプリングするLowResolutionLabelingSamplerInの基本解像度幅
     * この値は、samplingに渡すLowResolutionLabelingSamplerInに設定した値と同じである必要があります。
     * @param i_height
     * サンプリングするLowResolutionLabelingSamplerInの基本解像度高さ
     * この値は、samplingに渡すLowResolutionLabelingSamplerInに設定した値と同じである必要があります。
     * @param i_pix_size
     * 座標系の倍率係数を指定する。例えば1/2画像(面積1/4)のサンプリング結果を元画像サイズに戻すときは、4を指定する。
     * 最低解像度とするRasterのdepth。
     * この値は、samplingに渡すLowResolutionLabelingSamplerInに設定した値と同じである必要があります。
     * <p>メモ:ラスタ形式の多値化を考えるならアレだ。Impl作成。</p>
     * @throws NyARException
     */
    public LowResolutionLabelingSampler(int i_width, int i_height, int i_pix_size) {
      this._main_labeling = new Main_Labeling(i_width / i_pix_size, i_height / i_pix_size, i_pix_size);
    }

    /**
     * i_inのデータをサンプリングして、o_outにサンプル値を作成します。
     * この関数は、o_outにi_inのサンプリング結果を出力します。既にo_outにあるデータは初期化されます。
     * @param i_in
     * 入力元のデータです。
     * @param i_th
     * ラべリングの敷居値です。
     * @param o_out
     * 出力先のデータです。
     * @throws NyARException
     */
    public void Sampling(NyARGrayscaleRaster i_in, int i_th, LowResolutionLabelingSamplerOut o_out) {
      //クラスのパラメータ初期化
      Main_Labeling lb = this._main_labeling;
      lb.current_output = o_out;
      lb.current_th = i_th;

      //パラメータの設定
      o_out.initializeParams();

      //ラべリング
      lb.setAreaRange(10000, 3);
      lb.labeling(i_in, i_th);
    }
  }
}
