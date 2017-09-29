// ***********************************************************************
// Project          : Jinyinmao.AuthManager
// File             : Extension.cs
// Created          : 2016-12-28  12:34
//
// Last Modified By : 马新心(ma.xinxin@jinyinmao.com.cn)
// Last Modified On : 2016-12-28  12:34
// ***********************************************************************
// <copyright file="Extension.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using AutoMapper;

namespace Jinyinmao.AuthManager.Libraries.Extension
{
    /// <summary>
    ///     Class AutoMapperExtension.
    /// </summary>
    public static class AutoMapperExtension
    {
        /// <summary>
        ///     To the mapper.
        /// </summary>
        /// <typeparam name="TSource">The type of the t source.</typeparam>
        /// <typeparam name="TDestination">The type of the t destination.</typeparam>
        /// <param name="tSource">The t source.</param>
        /// <returns>TDestination.</returns>
        public static TDestination ToMapper<TSource, TDestination>(this TSource tSource) where TDestination : class, new() where TSource : class, new()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            return config.CreateMapper().Map(tSource, new TDestination());
        }
    }
}