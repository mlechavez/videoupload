/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    plugins = require('gulp-load-plugins')();

gulp.task('sass', function () {
    gulp.src('assets/sass/**/*.scss')
        .pipe(plugins.sourcemaps.init())
        .pipe(plugins.sass())
        .pipe(plugins.autoprefixer())
        .pipe(plugins.sourcemaps.write())
        .pipe(gulp.dest('assets/css'))
        .pipe(plugins.livereload());
});

gulp.task('watch', function () {
    plugins.livereload.listen();
    gulp.watch('assets/sass/**/*.scss', ['sass']);
});


gulp.task('default', ['watch']);

gulp.task('minifyFilesForRelease', function () {
    var cssFilter = plugins.filter('**/*.css', { restore: true });
    var jsFilter = plugins.filter('**/*.js', { restore: true });

    var assets = plugins.useref.assets();

    gulp.src('./**/*.cshtml')
        .pipe(assets)

    //Process JavaScript
        .pipe(jsFilter)
        .pipe(plugins.uglify())
        .pipe(plugins.rev())
        .pipe(assets.restore())
        .pipe(jsFilter.restore)
    //Process CSS
        .pipe(cssFilter)
        .pipe(plugins.minifyCss({
            keepSpecialComments: 0
        }))
        .pipe(plugins.rev())
        .pipe(assets.restore())
        .pipe(cssFilter.restore)

        .pipe(plugins.useref())
        .pipe(plugins.revReplace({
            replaceInExtensions: ['.js', '.css', '.html', '.cshtml']
        }))
        .pipe(gulp.dest(function (data) {
            return data.base;
        }));
});

//gulp.task('sass', function () {
//  return gulp.src('assets/sass/**/*.scss')
//    .pipe(sass())
//    .pipe(gulp.dest('assets/css'));
//});

//gulp.task('watch', ['sass'], function () {
//  gulp.watch('assets/sass/**/*.scss', ['sass']);
//});

//gulp.task('jquery', function () {
//    return gulp.src('bower_components/jquery/dist/**/*.js')
//      .pipe(gulp.dest('dist/js'));
//});

//gulp.task('bootstrapjs', function () {
//    return gulp.src('bower_components/bootstrap-sass/javascripts/bootstrap.min.js');
//});


//gulp.task('minifyForRelease', function () {
//    var cssFilter = plugins.filter('**/*.css', { restore: true });
//    var jsFilter = plugins.filter('**/*.js', { restore: true });

//    var assets = plugins.useref.assets();
//    gulp.src('./**/*.cshtml')
//        .pipe(assets)

//        //Process JavaScript
//        .pipe(jsFilter)
//        .pipe(plugins.uglify())
//        .pipe(plugins.rev())
//        .pipe(assets.restore())
//        .pipe(jsFilter.restore)

//        //Process CSS
//        .pipe(cssFilter)
//        .pipe(plugins.minifyCss({
//            keepSpecialComments: 0
//        }))
//        .pipe(plugins.rev())
//        .pipe(assets.restore())
//        .pipe(cssFilter.restore)

//        .pipe(plugins.useref())
//        .pipe(plugins.revReplace({
//            replaceInExtensions: ['.js', '.css', '.html', '.cshtml']
//        }))
//        .pipe(gulp.dest(function (data) {
//            return data.base;
//        }
//        ));           
//});

//gulp.task('images', function () {
//    return gulp.src('assets/images/**/*.+(png|jpg|jpeg|gif|svg')
//        .pipe(gulp.dest('dist/images'));
//});

//gulp.task('fonts', function () {
//    return gulp.src('assets/fonts/**/*')
//        .pipe(gulp.dest('dist/fonts'));
//});
//gulp.task('default', function () {
//    // place code for your default task here
//});