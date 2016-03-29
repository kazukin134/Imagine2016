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

using System.Diagnostics;
using NyAR.Core;

namespace NyAR.Rpf {

  /**
   * この関数は、NyARRgbRasterからコールします。
   */
  public class NyARGsRasterGraphicsFactory {
    /**
     * この関数は、i_rasterを操作するピクセルドライバインスタンスを生成します。
     * @param i_raster
     * @return
     * @throws NyARException
     */
    public static INyARGsRasterGraphics CreateDriver(INyARGrayscaleRaster i_raster) {
      switch (i_raster.getBufferType()) {
        case NyARBufferType.INT1D_GRAY_8:
          return new NyARGsRasterGraphics_GS_INT8(i_raster);
        default:
          break;
      }
      throw new NyARException();
    }
  }

  /**
   * このインタフェイスは、グレースケール画像に対するグラフィクス機能を定義します。
   */
  class NyARGsRasterGraphics_GS_INT8 : INyARGsRasterGraphics {
    private INyARGrayscaleRaster _raster;

    public NyARGsRasterGraphics_GS_INT8(INyARGrayscaleRaster i_raster) { this._raster = i_raster; }

    public void Fill(int i_value) {
      int[] buf = (int[])this._raster.getBuffer();
      NyARIntSize s = this._raster.getSize();
      for (int i = s.h * s.w - 1; i >= 0; i--) {
        buf[i] = i_value;
      }
    }

    public void CopyTo(int i_left, int i_top, int i_skip, INyARGrayscaleRaster o_output) {
      Debug.Assert(this._raster.getSize().isInnerSize(i_left + o_output.getWidth() * i_skip, i_top + o_output.getHeight() * i_skip));
      int[] input = (int[])this._raster.getBuffer();
      switch (o_output.getBufferType()) {
        case NyARBufferType.INT1D_GRAY_8:
          int[] output = (int[])o_output.getBuffer();
          NyARIntSize dest_size = o_output.getSize();
          NyARIntSize src_size = this._raster.getSize();
          int skip_src_y = (src_size.w - dest_size.w * i_skip) + src_size.w * (i_skip - 1);
          int pix_count = dest_size.w;
          int pix_mod_part = pix_count - (pix_count % 8);
          // 左上から1行づつ走査していく
          int pt_dst = 0;
          int pt_src = (i_top * src_size.w + i_left);
          for (int y = dest_size.h - 1; y >= 0; y -= 1) {
            int x;
            for (x = pix_count - 1; x >= pix_mod_part; x--) {
              output[pt_dst++] = input[pt_src];
              pt_src += i_skip;
            }
            for (; x >= 0; x -= 8) {
              output[pt_dst++] = input[pt_src];
              pt_src += i_skip;
              output[pt_dst++] = input[pt_src];
              pt_src += i_skip;
              output[pt_dst++] = input[pt_src];
              pt_src += i_skip;
              output[pt_dst++] = input[pt_src];
              pt_src += i_skip;
              output[pt_dst++] = input[pt_src];
              pt_src += i_skip;
              output[pt_dst++] = input[pt_src];
              pt_src += i_skip;
              output[pt_dst++] = input[pt_src];
              pt_src += i_skip;
              output[pt_dst++] = input[pt_src];
              pt_src += i_skip;
            }
            // スキップ
            pt_src += skip_src_y;
          }
          return;
        default:
          throw new NyARException();
      }
    }
  }
}
